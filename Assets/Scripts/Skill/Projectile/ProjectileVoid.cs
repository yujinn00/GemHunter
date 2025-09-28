using UnityEngine;

public class ProjectileVoid : ProjectileGlobal
{
    // Void 본체가 주기적으로 생성하는 발사체 프리팹.
    [SerializeField]
    private GameObject projectile;

    // Void 본체가 주기적으로 생성하는 발사체의 방향 설정을 위한 반지름.
    private float radius = 1f;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);

        // Void 본체 이동 방향 설정.
        Vector3 targetPosition = skillBase.Owner.Target.MiddlePoint;
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        movementRigidbody2D.MoveTo((targetPosition - transform.position).normalized);
    }

    public override void Process()
    {
        base.Process();

        if (Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for (int i = 0; i < skillBase.GetStat(StatType.ProjectileCount).Value; ++i)
            {
                // Void 발사체 생성 및 위치 설정.
                float angle = 360 / skillBase.GetStat(StatType.ProjectileCount).Value * i;
                Vector3 position = transform.position + Utils.GetPositionFromAngle(radius, angle);
                GameObject clone = Instantiate(projectile, position, Quaternion.identity);

                // Void 발사체 이동 방향 설정.
                Vector3 direction = (clone.transform.position - transform.position).normalized;
                clone.GetComponent<MovementRigidbody2D>().MoveTo(direction);

                // Void 발사체 공격력 설정.
                clone.GetComponent<ProjectileCollision2D>().Setup(skillBase.Owner.Target, damage);
            }

            currentAttackRate = Time.time;
        }
    }
}
