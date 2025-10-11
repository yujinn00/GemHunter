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
        // cf) stats �迭�� �ִ� ��� ������ Stats.GetStat() �޼ҵ带 �̿��� ���ϴ� ���ȿ� ������.
        Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        // Evasion ���� Ȯ���� ���� ȸ��.
        if (Random.value < Stats.GetStat(StatType.Evasion).Value)
        {
            return;
        }

        // Stat Ŭ������ DefaultValue ������Ƽ�� min/max �˻簡 ���ԵǾ� �ֱ� ������,
        // ����� ���� ������ �˻� ���� ���� ������.
        // cf) �Ʒ��� �ڵ�� �����ϱ� �������� ���⼭ �˻縦 �߾���.
        Stats.CurrentHP.DefaultValue -= damage;

        if (Mathf.Approximately(Stats.CurrentHP.DefaultValue, 0f))
        {
            // Entity ��� ó��.
            OnDie();
        }
    }

    // ������� ���� ó���� �÷��̾�� ���� ���� �ٸ��� ������,
    // �ڽ� Ŭ�������� �������ϵ��� �߻� �޼ҵ带 ������.
    protected abstract void OnDie();
}
