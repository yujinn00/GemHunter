using UnityEngine;

public class ProjectileGlobal : ProjectileBase
{
    // ���� ��ų�� �ǰݴ����� �� ����� �ǰ� ����Ʈ ������.
    [SerializeField]
    private Transform hitEffect;
    // �ǰ� �������� ����� Text UI ������.
    [SerializeField]
    private UIDamageText damageText;

    // ���� ��ų�� ����.
    protected SkillBase skillBase;
    // ���� ��ų�� ���� �ð� ����� ���� ����.
    protected float currentDuration;
    // ���� ��ų�� ���� �ֱ� ����� ���� ����.
    protected float currentAttackRate = 0;
    // ���� ��ų�� ���ݷ�.
    protected float damage;

    public override void Setup(SkillBase skillBase, float damage)
    {
        this.skillBase = skillBase;
        this.damage = damage;

        // ���� ��ų�� ���� �ð� ����� ���� ���� �ð��� ����.
        currentDuration = Time.time;
    }

    /// <summary>
    /// ���� ��ų ���� 1: ���� �ð��� ����, 1ȸ�� ������ ���� ����.
    /// ���� ��ų ���� 2: ���� �ð��� �ְ�, �� �ð� ���� ������ ���� ����.
    /// ���� �ð�(Duration)�� �ִ� ��ų�� base.Process() ȣ��.
    /// </summary>
    public override void Process()
    {
        // ���� �ð��� ���� ��ų�� base.Process()�� �������� ��, �������� �ʵ��� ���� ó��.
        if (skillBase.GetStat(StatType.Duration) == null)
        {
            return;
        }

        // �߻�ü ���� ����(currentDuration)���� StatType.Duration �ð��� ������ �߻�ü ����.
        if (Time.time - currentDuration > skillBase.GetStat(StatType.Duration).Value)
        {
            Destroy(gameObject);
        }
    }

    protected void TakeDamage(EntityBase entity)
    {
        // �ǰ� ȿ�� ������Ʈ�� ������, �Ű������� �޾ƿ� entity�� �߽� ��ġ�� �ǰ� ȿ���� ����.
        if (hitEffect != null)
        {
            Instantiate(hitEffect, entity.MiddlePoint, Quaternion.identity);
        }

        // ������ Text UI ������Ʈ�� ������ entity�� �߽� ��ġ�� ������ Text UI�� ����.
        if (damageText != null)
        {
            UIDamageText clone = Instantiate(damageText, entity.MiddlePoint, Quaternion.identity);
            clone.Setup(damage.ToString("F0"), Color.white);
        }

        // entity�� TakeDamage() �޼ҵ带 ȣ���� ü���� damage��ŭ ���ҽ�Ŵ.
        entity.TakeDamage(damage);
    }
}
