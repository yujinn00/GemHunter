using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMark;

    // 스킬 사용 등 여러 곳에서 사용하기 때문에 public 프로퍼티로 정의.
    public bool IsMoved { get; set; } = false;

    private void Awake()
    {
        base.Setup();
    }

    private void Update()
    {
        if (Target == null)
        {
            targetMark.gameObject.SetActive(false);
        }

        SearchTarget();
        Recovery();
    }

    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach (var entity in EnemySpawner.Enemies)
        {
            // 제일 가까운 대상을 찾기 위해 sqrMagnitude 사용.
            float distance = (entity.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistSqr)
            {
                closestDistSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if (Target != null)
        {
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position;
            targetMark.gameObject.SetActive(true);
        }
    }

    private void Recovery()
    {
        // 체력 회복.
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
        {
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        }
        else
        {
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
        }
    }
}
