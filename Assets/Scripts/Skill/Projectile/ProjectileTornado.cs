using UnityEngine;

public class ProjectileTornado : ProjectileGlobal
{
    [SerializeField]
    private float attackRadius;     // ����̵��� ���� ���� ������.
    [SerializeField]
    private LayerMask targetLayer;  // ����̵��� ������ �� �ִ� ���̾�.

    private EntityBase target;      // ����̵��� �����ϴ� ���.

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);

        // ����̵��� �ʵ带 �̵��ϱ� ������ MovementRigidbody2D ������Ʈ ���.
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        // ����̵��� ù ��° ��ǥ ����.
        target = skillBase.Owner.Target;
    }

    public override void Process()
    {
        base.Process();

        if (target == null)
        {
            SearchClosestTarget();
            return;
        }

        // ��ǥ(target) ��ġ�� �̵�.
        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);

        // AttackRate �ð����� ����̵��� ����(attackRadius) ���� �ִ� ��� �� ����.
        if (Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            Collider2D[] entities = Physics2D.OverlapCircleAll(transform.position, attackRadius, targetLayer);
            for (int i = 0; i < entities.Length; ++i)
            {
                if (entities[i].TryGetComponent<EntityBase>(out var entity))
                {
                    TakeDamage(entity);
                }
            }

            currentAttackRate = Time.time;
        }
    }

    private void SearchClosestTarget()
    {
        // �ش� ���� ����̵� �߻�ü�� �Ÿ��� ����ϰ�, �Ÿ��� ���� ª�� ���� ���ο� target���� ����.
        float closestDistSqr = Mathf.Infinity;
        foreach (var entity in EnemySpawner.Enemies)
        {
            float distance = (entity.transform.position - transform.position).sqrMagnitude;
            if (distance < closestDistSqr)
            {
                closestDistSqr = distance;
                target = entity.GetComponent<EntityBase>();
            }
        }
    }
}
