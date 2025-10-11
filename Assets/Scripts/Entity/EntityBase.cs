using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class EntityBase : MonoBehaviour
{
    [SerializeField]
    private EntityStats stats;
    [SerializeField]
    private Transform middlePoint;

    public EntityStats Stats => stats;
    public bool isDead => stats.CurrentHP != null && Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f);
    public Vector3 MiddlePoint => middlePoint != null ? middlePoint.position : Vector3.zero;
    public EntityBase Target { get; set; }

    protected virtual void Setup()
    {
        // cf) stats 배열에 있는 모든 스탯은 Stats.GetStat() 메소드를 이용해 원하는 스탯에 접근함.
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // Evasion 스탯 확률로 공격 회피.
        if (Random.value < Stats.GetStat(StatType.Evasion).Value)
        {
            return;
        }

        // Stat 클래스의 DefaultValue 프로퍼티에 min/max 검사가 포함되어 있기 때문에,
        // 사용할 때는 별도의 검사 없이 값을 설정함.
        // cf) 아래의 코드로 변경하기 전까지는 여기서 검사를 했었음.
        Stats.CurrentHP.DefaultValue -= damage;

        if (Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f))
        {
            // Entity 사망 처리.
            OnDie();
        }
    }

    // 사망했을 때의 처리는 플레이어와 적에 따라 다르기 때문에,
    // 자식 클래스에서 재정의하도록 추상 메소드를 정의함.
    protected abstract void OnDie();
}
