using System.Collections.Generic;
using UnityEngine;

public class UISkillList : MonoBehaviour
{
    [SerializeField]
    private UISkillIcon skillIconPrefab;                    // 스킬 아이콘 생성을 위한 프리팹.
    [SerializeField]
    private Transform[] skillElementType;                   // 스킬 아이콘을 속성별로 정리하기 위한 부모 Transform 배열.
    [SerializeField]
    private Transform skillElemetnBonus;                    // 속성 스킬 아이콘을 정리하기 위한 부모 Transform.

    private Dictionary<string, UISkillIcon> skillIcons;     // 모든 스킬 아이콘을 관리하는 변수.

    public void Setup(Dictionary<string, SkillTemplate> skills, Dictionary<string, SkillTemplate> elementalBonus)
    {
        skillIcons = new Dictionary<string, UISkillIcon>();

        foreach (var item in skills)
        {
            SpawnIcon(item.Value, skillElementType[(int)item.Value.element - 100]);
        }

        foreach (var item in elementalBonus)
        {
            SpawnIcon(item.Value, skillElemetnBonus);
        }
    }

    public void LevelUp(SkillBase skill)
    {
        if (skillIcons.ContainsKey(skill.SkillName))
        {
            skillIcons[skill.SkillName].LevelUp(skill.CurrentLevel, skill.EnableIcon);
        }
    }

    private void SpawnIcon(SkillTemplate skill, Transform parent)
    {
        var clone = Instantiate(skillIconPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.Setup(skill.disableIcon);

        skillIcons.Add(skill.skillName, clone);
    }
}
