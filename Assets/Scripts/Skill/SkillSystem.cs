using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [SerializeField]
    private SkillGad skillGad;
    [SerializeField]
    private Transform skillSpawnPoint;

    private PlayerBase owner;

    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private Dictionary<SkillElement, int> elementalCounts = new Dictionary<SkillElement, int>();
    private Dictionary<SkillElement, SkillBase> elementalSkills = new Dictionary<SkillElement, SkillBase>();

    private void Awake()
    {
        owner = GetComponent<PlayerBase>();
        skillGad.Setup(owner, skillSpawnPoint);

        // Resources/Skills/ ������ �����ص� ��� ��ų ������ �ҷ��� ��ų ���� <���� �̸�, SkillTemplate>.
        var skillDict = Resources.LoadAll<SkillTemplate>("Skills/").ToDictionary(item => item.name, item => item);
        foreach (var item in skillDict)
        {
            SkillBase skill = null;
            if (item.Value.skillType.Equals(SkillType.Buff))
            {
                skill = new SkillBuff();
            }
            else if (item.Value.skillType.Equals(SkillType.Emission))
            {
                skill = new SkillEmission();
            }
            else if (item.Value.skillType.Equals(SkillType.Sustained))
            {
                skill = new SkillSustained();
            }
            else if (item.Value.skillType.Equals(SkillType.Global))
            {
                skill = new SkillGlobal();
            }

            skill.Setup(item.Value, owner, skillSpawnPoint);
            skills.Add(item.Key, skill);

            // ������ ��� ��ų�� �̸�, ����, ���� ��� [Debug].
            Logger.Log($"[{skill.SkillName}] Lv. {skill.CurrentLevel}\n{skill.Description}");
        }

        // �Ӽ� ���ʽ� ��ų ���.
        var eSkillDict = Resources.LoadAll<SkillTemplate>("ElementalSkills/").ToDictionary(item => item.name, item => item);
        foreach (var item in eSkillDict)
        {
            SkillBase skill = new SkillBuff();
            skill.Setup(item.Value, owner, skillSpawnPoint);

            elementalCounts.Add(item.Value.element, 0);         // �� �Ӽ� ��ų�� ���� ī��Ʈ.
            elementalSkills.Add(item.Value.element, skill);     // �� �Ӽ� ���ʽ� ��ų (SkillBase).

            Logger.Log($"{item.Value.element}, {item.Value.skillName}");
        }
    }

    private void Update()
    {
        // ������ ������ ������ ��ų 3���� �����ϰ�, �� �� �ϳ��� ������ [Debug Test].
        if (UnityEngine.InputSystem.Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectSkill();
        }

        // ��� ���� ��ų ������Ʈ.
        foreach (var item in skills)
        {
            if (item.Value.CurrentLevel == 0)
            {
                continue;
            }

            item.Value.OnSkill();
        }

        // �÷��̾��� ��ǥ�� ���ų� || �̵� ���̸� ��� ��ų ���� �Ұ�.
        if (owner.Target == null || owner.IsMoved == true)
        {
            return;
        }

        // �⺻ ���� ��ų ������Ʈ.
        skillGad.OnSkill();

        // ��� ���� ��ų�� ��Ÿ�� ������Ʈ.
        foreach (var item in skills)
        {
            item.Value.IsSkillAvailable();
        }
    }

    public void LevelUp(SkillBase skill)
    {
        if (skills.ContainsValue(skill))
        {
            skill.TryLevelUp();
            Logger.Log($"Level Up [{skill.SkillName}] {skill.Element}, Lv. {skill.CurrentLevel}");

            // �ش� ��ų�� �Ҽӵ� �Ӽ��� �� ��ų ���� �� +1.
            elementalCounts[skill.Element]++;

            // �ش� ��ų�� �Ӽ� ���ʽ� ���� ���� ���� �Ǵ�.
            if (elementalCounts[skill.Element] % 3 == 0)
            {
                elementalSkills[skill.Element].TryLevelUp();
                Logger.Log($"{skill.Element} Lv. {elementalSkills[skill.Element].CurrentLevel}");
            }
        }
    }

    public void SelectSkill()
    {
        // ���� or ������ ������ ������ 3�� ��ų ����.
        var randomSkills = GetRandomSkills(skills, 3);
        if (randomSkills == null)
        {
            Logger.Log("�� �̻� ������ �� �ִ� ��ų�� �����ϴ�.");
            return;
        }

        // ��ų ���� UI�� ���� ������ �ӽ÷� ��ų ���� ó��.
        int index = Random.Range(0, randomSkills.Count);
        LevelUp(randomSkills[index]);
    }

    private List<SkillBase> GetRandomSkills(Dictionary<string, SkillBase> skills, int count = 3)
    {
        // ���� ������ ��ų ���.
        var values = new List<SkillBase>(skills.Values.Where(skill => !skill.IsMaxLevel)).ToList();
        var randomSkills = new List<SkillBase>();

        count = values.Count == 0 ? 0 : count;

        if (count == 0)
        {
            return null;
        }

        for (int i = 0; i < count; ++i)
        {
            int index = Random.Range(0, values.Count);

            // index��° ������ �׸� ����.
            randomSkills.Add(values[index]);

            // �ߺ� ������ ���� ���õ� �׸� ����.
            values.RemoveAt(index);
        }

        Logger.Log($"���� ������ 3���� ��ų\n{randomSkills[0].SkillName}," + $"{randomSkills[1].SkillName}, {randomSkills[2].SkillName}");

        return randomSkills;
    }
}
