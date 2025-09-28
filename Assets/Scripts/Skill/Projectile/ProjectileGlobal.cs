using UnityEngine;

public class ProjectileGlobal : ProjectileBase
{
    // 광역 스킬에 피격당했을 때 재생할 피격 이펙트 프리팹.
    [SerializeField]
    private Transform hitEffect;
    // 피격 데미지를 출력할 Text UI 프리팹.
    [SerializeField]
    private UIDamageText damageText;

    // 광역 스킬의 정보.
    protected SkillBase skillBase;
    // 광역 스킬의 지속 시간 계산을 위한 변수.
    protected float currentDuration;
    // 광역 스킬의 공격 주기 계산을 위한 변수.
    protected float currentAttackRate = 0;
    // 광역 스킬의 공격력.
    protected float damage;

    public override void Setup(SkillBase skillBase, float damage)
    {
        this.skillBase = skillBase;
        this.damage = damage;

        // 광역 스킬의 지속 시간 계산을 위해 현재 시간을 저장.
        currentDuration = Time.time;
    }

    /// <summary>
    /// 광역 스킬 종류 1: 지속 시간이 없고, 1회만 공격할 수도 있음.
    /// 광역 스킬 종류 2: 지속 시간이 있고, 그 시간 동안 공격할 수도 있음.
    /// 지속 시간(Duration)이 있는 스킬만 base.Process() 호출.
    /// </summary>
    public override void Process()
    {
        // 지속 시간이 없는 스킬이 base.Process()를 실행했을 때, 실행하지 않도록 예외 처리.
        if (skillBase.GetStat(StatType.Duration) == null)
        {
            return;
        }

        // 발사체 생성 시점(currentDuration)부터 StatType.Duration 시간이 지나면 발사체 삭제.
        if (Time.time - currentDuration > skillBase.GetStat(StatType.Duration).Value)
        {
            Destroy(gameObject);
        }
    }

    protected void TakeDamage(EntityBase entity)
    {
        // 피격 효과 오브젝트가 있으면, 매개변수로 받아온 entity의 중심 위치에 피격 효과를 생성.
        if (hitEffect != null)
        {
            Instantiate(hitEffect, entity.MiddlePoint, Quaternion.identity);
        }

        // 데미지 Text UI 오브젝트가 있으면 entity의 중심 위치에 데미지 Text UI를 생성.
        if (damageText != null)
        {
            UIDamageText clone = Instantiate(damageText, entity.MiddlePoint, Quaternion.identity);
            clone.Setup(damage.ToString("F0"), Color.white);
        }

        // entity의 TakeDamage() 메소드를 호출해 체력을 damage만큼 감소시킴.
        entity.TakeDamage(damage);
    }
}
