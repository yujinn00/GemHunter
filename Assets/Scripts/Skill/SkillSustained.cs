using System.Collections.Generic;
using UnityEngine;

public class SkillSustained : SkillBase
{
    // ������ �߻�ü�� �÷��̾��� �Ÿ� ����.
    private float distanceToPlayer = 2f;
    // ������ �߻�ü�� �θ� Transform ������ ������ �ִ� ����.
    private Transform parent;
    // ��� ������ �߻�ü�� �����ϴ� ����.
    private List<GameObject> pickaxs = new List<GameObject>();

    public override void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        base.Setup(skillTemplate, owner, spawnPoint);

        // �̸� �����ص� ��̵��� �θ� ������Ʈ.
        parent = GameObject.Find("Pickaxs").transform;
    }

    public override void OnLevelUp()
    {
        // ������ 0���� 1�� ���� �ÿ��� ������ �������� �ʰ�, ��� ������Ʈ�� ����.
        if (currentLevel <= 1)
        {
            AddPickax((int)GetStat(StatType.ProjectileCount).Value);

            // ���� Ȱ��ȭ�Ǿ� �ִ� ��� ����� ��ġ �缳��.
            int pickaxCount = parent.childCount;
            for (int i = 0; i < pickaxCount; ++i)
            {
                float angle = (360 / pickaxCount) * i;
                Vector3 position = Utils.GetPositionFromAngle(distanceToPlayer, angle);
                parent.GetChild(i).position = parent.position + position;
            }

            return;
        }

        // ���� ��ų ������ �� ���ݷ� �� ���� ����.
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });

        // ��� ����� ���ݷ� ����.
        foreach (var item in pickaxs)
        {
            item.GetComponent<ProjectileCollision2D>().Setup(null, GetStat(StatType.Damage).Value);
        }
    }

    private void AddPickax(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject clone = GameObject.Instantiate(skillTemplate.projectile, parent);
            clone.GetComponent<ProjectileCollision2D>().Setup(null, GetStat(StatType.Damage).Value);
            pickaxs.Add(clone);
        }
    }

    public override void OnSkill() { }
}
