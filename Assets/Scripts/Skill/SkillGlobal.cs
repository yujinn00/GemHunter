using UnityEngine;

public class SkillGlobal : SkillBase
{
    public override void OnLevelUp()
    {
        // 레벨이 0에서 1로 습득 시에는 스탯이 증가하지 않도록 설정.
        if (currentLevel <= 1)
        {
            return;
        }

        // 공격 스킬 레벨업 시 공격력 등 스탯 갱신.
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });
    }

    public override void OnSkill()
    {
        // 스킬이 사용 가능한 상태인지 검사 (쿨타임).
        if (isSkillAvailable == true)
        {
            // Global 스킬 발사체 생성.
            GameObject projectile = GameObject.Instantiate(skillTemplate.projectile, spawnPoint.position, Quaternion.identity);
            projectile.GetComponent<ProjectileGlobal>().Setup(this, GetStat(StatType.Damage).Value);

            // 쿨타임 재계산.
            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }
}
