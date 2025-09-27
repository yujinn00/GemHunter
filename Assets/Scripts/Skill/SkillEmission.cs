using UnityEngine;

public class SkillEmission : SkillBase
{
    private float attackRate = 0.05f;
    private float currentAttackRate = 0;
    private int currentProjectileCount = 0;

    public override void OnLevelUp()
    {
        // ������ 0���� 1�� ���� �ÿ��� ������ �������� �ʵ��� ����.
        if (currentLevel <= 1)
        {
            return;
        }

        // ���� ��ų ������ �� ���ݷ� �� ���� ����.
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }

    public override void OnSkill()
    {
        // ��ų�� ��� ������ �������� �˻� (��Ÿ��).
        if (isSkillAvailable == true)
        {
            int maxCount = (int)GetStat(StatType.ProjectileCount).Value;

            // attackRate �ֱ�� �߻�ü ����.
            if (Time.time - currentAttackRate > attackRate)
            {
                GameObject projectile = GameObject.Instantiate(skillTemplate.projectile, spawnPoint.position, Quaternion.identity);

                // ProjectileStraight, ProjectileHoming�� ������ ����������,
                // 3, 4��° �Ű������� �ʿ� ���� ������ ������ �����ϰ� ó��.
                if (projectile.TryGetComponent<ProjectileCubicHoming>(out var p))
                {
                    p.Setup(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
                }
                else if (projectile.TryGetComponent<ProjectileQuadraticHoming>(out var p2))
                {
                    p2.Setup(owner.Target, GetStat(StatType.Damage).Value, maxCount, currentProjectileCount);
                }
                else
                {
                    projectile.GetComponent<ProjectileBase>().Setup(owner.Target, GetStat(StatType.Damage).Value);
                }

                currentProjectileCount++;
                currentAttackRate = Time.time;
            }

            // ProjectileCount ������ŭ �߻�ü�� ������ �� ��Ÿ�� �ʱ�ȭ.
            if (currentProjectileCount >= maxCount)
            {
                isSkillAvailable = false;
                currentProjectileCount = 0;
                currentCooldownTime = Time.time;
            }
        }
    }
}
