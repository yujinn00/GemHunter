using UnityEngine;

public class SkillGlobal : SkillBase
{
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
            // Global ��ų �߻�ü ����.
            GameObject projectile = GameObject.Instantiate(skillTemplate.projectile, spawnPoint.position, Quaternion.identity);
            projectile.GetComponent<ProjectileGlobal>().Setup(this, GetStat(StatType.Damage).Value);

            // ��Ÿ�� ����.
            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }
}
