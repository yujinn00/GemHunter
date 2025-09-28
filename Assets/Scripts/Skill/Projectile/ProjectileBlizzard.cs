using UnityEngine;

public class ProjectileBlizzard : ProjectileGlobal
{
    public override void Process()
    {
        base.Process();

        // ������ ����Ʈ�� ȭ�� ��ü�� ��� ��µǵ��� �������� ��ġ�� �÷��̾� ��ġ�� �����ϰ� ����.
        transform.position = skillBase.Owner.transform.position;

        // AttackRate �ð����� ���忡 �ִ� ��� �� ����.
        if (Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for (int i = 0; i < EnemySpawner.Enemies.Count; ++i)
            {
                // �ٸ� ���ݿ� ���� ����� ���� �ֱ� ������ i��° ���� null�̸� �ǳʶ�.
                if (EnemySpawner.Enemies[i] == null)
                {
                    continue;
                }

                // i��° �� ������ �Ű������� TakeDamage() �޼ҵ带 ȣ���� ü���� ���ҽ�Ŵ.
                TakeDamage(EnemySpawner.Enemies[i]);
            }

            // ��� ���� ���� ������ �Ϸ�Ǹ�, currentAttackRate�� ���� �ð��� ������.
            currentAttackRate = Time.time;
        }
    }
}
