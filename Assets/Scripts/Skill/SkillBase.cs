using System.Linq;
using UnityEngine;

public abstract class SkillBase
{
    protected SkillTemplate skillTemplate;      // ��ų ����.
    protected PlayerBase owner;                 // ��ų ������.
    protected Transform spawnPoint;             // ��ų �߻� ��ġ.
    protected int currentLevel = 0;             // ���� ��ų ����.
    protected float currentCooldownTime = 0;    // ���� ��ų ��Ÿ��.
    protected bool isSkillAvailable = false;    // ���� ��ų ��� ���� ����.

    // �ܺο��� ������ �ʿ��� ��ų�� �������� Get ������Ƽ�� ����.
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;
    public PlayerBase Owner => owner;

    // ���� ��ų ���� (���ݷ�, ��Ÿ��, �߻�ü ������ ���� ����).
    private Stat[] stats;
    public Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
        this.spawnPoint = spawnPoint;

        // ���� ��ų�� ��� ���ݿ� �ʿ��� ���� ���� (���ݷ�, ��Ÿ��, �߻�ü ���� ��).
        if (SkillType != SkillType.Buff)
        {
            stats = new Stat[skillTemplate.attackBaseStats.Count];
            for (int i = 0; i < stats.Length; ++i)
            {
                stats[i] = new Stat();
                stats[i].CopyData(skillTemplate.attackBaseStats[i]);
            }
        }
    }

    public void TryLevelUp()
    {
        if (IsMaxLevel)
        {
            Logger.Log($"[{SkillName}] ��ų �ְ� ���� ����");
            return;
        }

        currentLevel++;

        OnLevelUp();
    }

    public void IsSkillAvailable()
    {
        // ������ 0�̰ų� ���� �Ǵ� ���� ��ų�� ���� ��ų ��� ���� ���� X.
        if (CurrentLevel == 0 || SkillType == SkillType.Buff || SkillType == SkillType.Sustained)
        {
            return;
        }

        if (Time.time - currentCooldownTime > GetStat(StatType.CooldownTime).Value)
        {
            isSkillAvailable = true;
        }
    }

    protected float CalculateDamage()
    {
        float damage = GetStat(StatType.Damage).Value;
        damage += damage * owner.Stats.GetStat((StatType)Element).Value;

        return damage;
    }

    public abstract void OnLevelUp();   // ��ų ������ �� 1ȸ ȣ��.
    public abstract void OnSkill();     // ��ų ��� �� ȣ��.
}
