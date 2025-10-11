using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyBase : EntityBase
{
    [SerializeField]
    private Transform hudPoint;
    [SerializeField]
    private GameObject uiPrefab;

    private EnemySpawner enemySpawner;

    private void Awake()
    {
        Setup();
    }

    protected override void Setup()
    {
        // 기본 체력은 DefaultValue에 해당하기 때문에,
        // 기존 코드와 같이 100이란 매직 넘버를 사용하지 않고, BonusValue만 설정함.
        // cf) 기존 코드에는 우항 값에 100을 더했었음.
        Stats.GetStat(StatType.HP).BonusValue = 50 * (Stats.GetStat(StatType.Level).Value - 1);

        base.Setup();
    }

    public void Initialize(EnemySpawner enemySpawner, Transform parent)
    {
        this.enemySpawner = enemySpawner;

        GameObject clone = Instantiate(uiPrefab, parent);
        clone.transform.localScale = Vector3.one;
        clone.GetComponent<FollowTargetUI>().Setup(hudPoint);
        clone.GetComponentInChildren<UIHP>().Setup(this);
    }

    protected override void OnDie()
    {
        // 적은 레벨업을 하지 않기 때문에 적의 경험치 스탯만큼 플레이어 경험치 증가.
        // 적의 목표(Target)는 플레이어이기 때문에 EntityBase 타입의 Target을 PlayerBase로 형 변환하고,
        // AccumulationExp 프로퍼티에 적이 소지하고 있는 경험치(Stats.CurrentExp.Value)를 더해줌.
        (Target as PlayerBase).AccumulationExp += Stats.CurrentExp.Value;

        // 적 본인(this) 사망 처리.
        enemySpawner.Deactivate(this);
    }
}
