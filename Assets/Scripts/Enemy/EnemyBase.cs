using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private GameObject uiPrefab;

    private void Awake()
    {
        Setup();
    }

    protected override void Setup()
    {
        // 기본 체력은 DefaultValue에 해당하기 때문에,
        // 기존 코드와 같이 100이란 매직 넘버를 사용하지 않고, BonusValue만 설정함.
        // cf) 기존 코드에는 우항 값에 100을 더했었음.
        Stats.GetStat(StatType.HP).BonusValue = 50 * (Stats.level - 1);

        base.Setup();
    }

    public void Initialize(Transform parent)
    {
        GameObject clone = Instantiate(uiPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint);
        clone.GetComponentInChildren<UIHP>().Setup(this);
    }
}
