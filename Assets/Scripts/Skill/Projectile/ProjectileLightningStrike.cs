using UnityEngine;

public class ProjectileLightningStrike : ProjectileGlobal
{
    // �ǰ� ���ϴ� ��� ��ġ�� ����� ���� ȿ�� ������.
    [SerializeField]
    private GameObject projectile;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);
    }

    public override void Process()
    {
        for (int i = 0; i < EnemySpawner.Enemies.Count; ++i)
        {
            if (EnemySpawner.Enemies[i] == null)
            {
                return;
            }

            // Enemies[i] ���� ��Ÿ�ϴ� ���� ����Ʈ ����.
            Instantiate(projectile, EnemySpawner.Enemies[i].MiddlePoint, Quaternion.identity);
            TakeDamage(EnemySpawner.Enemies[i]);
        }

        Destroy(gameObject);
    }
}
