using System.Linq;
using UnityEngine;

public abstract class SkillBase
{
    protected SkillTemplate skillTemplate;      // 스킬 정보.
    protected PlayerBase owner;                 // 스킬 소유자.
    protected Transform spawnPoint;             // 스킬 발사 위치.
    protected int currentLevel = 0;             // 현재 스킬 레벨.
    protected float currentCooldownTime = 0;    // 현재 스킬 쿨타임.
    protected bool isSkillAvailable = false;    // 현재 스킬 사용 가능 여부.

    // 외부에서 접근이 필요한 스킬의 정보들을 Get 프로퍼티로 정의.
    public string SkillName => skillTemplate.skillName;
    public SkillType SkillType => skillTemplate.skillType;
    public SkillElement Element => skillTemplate.element;
    public string Description => skillTemplate.description;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel == skillTemplate.maxLevel;
    public PlayerBase Owner => owner;

    // 공격 스킬 전용 (공격력, 쿨타임, 발사체 개수와 같은 스탯).
    private Stat[] stats;
    public Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);

    public virtual void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        this.skillTemplate = skillTemplate;
        this.owner = owner;
        this.spawnPoint = spawnPoint;

        // 공격 스킬인 경우 공격에 필요한 스탯 설정 (공격력, 쿨타임, 발사체 개수 등).
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
            Logger.Log($"[{SkillName}] 스킬 최고 레벨 도달");
            return;
        }

        currentLevel++;

        OnLevelUp();
    }

    public void IsSkillAvailable()
    {
        // 레벨이 0이거나 버프 또는 지속 스킬인 경우는 스킬 사용 가능 상태 X.
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

    public abstract void OnLevelUp();   // 스킬 레벨업 시 1회 호출.
    public abstract void OnSkill();     // 스킬 사용 시 호출.
}
