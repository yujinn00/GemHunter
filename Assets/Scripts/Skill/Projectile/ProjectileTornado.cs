using UnityEngine;

public class ProjectileTornado : ProjectileGlobal
{
    [SerializeField]
    private float attackRadius;     // 토네이도의 공격 범위 반지름.
    [SerializeField]
    private LayerMask targetLayer;  // 토네이도가 공격할 수 있는 레이어.

    private EntityBase target;      // 토네이도가 추적하는 대상.

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);

        // 토네이도는 필드를 이동하기 때문에 MovementRigidbody2D 컴포넌트 사용.
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        // 토네이도의 첫 번째 목표 설정.
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

        // 목표(target) 위치로 이동.
        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);

        // AttackRate 시간마다 토네이도의 범위(attackRadius) 내에 있는 모든 적 공격.
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
        // 해당 적과 토네이도 발사체의 거리를 계산하고, 거리가 가장 짧은 적을 새로운 target으로 설정.
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
