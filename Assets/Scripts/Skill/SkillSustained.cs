using System.Collections.Generic;
using UnityEngine;

public class SkillSustained : SkillBase
{
    // 지속형 발사체와 플레이어의 거리 변수.
    private float distanceToPlayer = 2f;
    // 지속형 발사체의 부모 Transform 정보를 가지고 있는 변수.
    private Transform parent;
    // 모든 지속형 발사체를 관리하는 변수.
    private List<GameObject> pickaxs = new List<GameObject>();

    public override void Setup(SkillTemplate skillTemplate, PlayerBase owner, Transform spawnPoint = null)
    {
        base.Setup(skillTemplate, owner, spawnPoint);

        // 미리 제작해둔 곡괭이들의 부모 오브젝트.
        parent = GameObject.Find("Pickaxs").transform;
    }

    public override void OnLevelUp()
    {
        // 레벨이 0에서 1로 습득 시에는 스탯은 증가하지 않고, 곡괭이 오브젝트만 생성.
        if (currentLevel <= 1)
        {
            AddPickax((int)GetStat(StatType.ProjectileCount).Value);

            // 현재 활성화되어 있는 모든 곡괭이의 위치 재설정.
            int pickaxCount = parent.childCount;
            for (int i = 0; i < pickaxCount; ++i)
            {
                float angle = (360 / pickaxCount) * i;
                Vector3 position = Utils.GetPositionFromAngle(distanceToPlayer, angle);
                parent.GetChild(i).position = parent.position + position;
            }

            return;
        }

        // 공격 스킬 레벨업 시 공격력 등 스탯 갱신.
        skillTemplate.attackBuffStats.ForEach(stat =>
        {
            GetStat(stat).BonusValue += stat.DefaultValue;
        });

        // 모든 곡괭이의 공격력 갱신.
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
