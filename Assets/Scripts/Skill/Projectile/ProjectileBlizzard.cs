using UnityEngine;

public class ProjectileBlizzard : ProjectileGlobal
{
    public override void Process()
    {
        base.Process();

        // 눈보라 이펙트가 화면 전체에 계속 출력되도록 눈보라의 위치를 플레이어 위치와 동일하게 설정.
        transform.position = skillBase.Owner.transform.position;

        // AttackRate 시간마다 월드에 있는 모든 적 공격.
        if (Time.time - currentAttackRate > skillBase.GetStat(StatType.AttackRate).Value)
        {
            for (int i = 0; i < EnemySpawner.Enemies.Count; ++i)
            {
                // 다른 공격에 의해 사망할 수도 있기 때문에 i번째 적이 null이면 건너뜀.
                if (EnemySpawner.Enemies[i] == null)
                {
                    continue;
                }

                // i번째 적 정보를 매개변수로 TakeDamage() 메소드를 호출해 체력을 감소시킴.
                TakeDamage(EnemySpawner.Enemies[i]);
            }

            // 모든 적에 대해 공격이 완료되면, currentAttackRate에 현재 시간을 저장함.
            currentAttackRate = Time.time;
        }
    }
}
