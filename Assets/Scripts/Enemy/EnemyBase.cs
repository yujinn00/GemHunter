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
        // �⺻ ü���� DefaultValue�� �ش��ϱ� ������,
        // ���� �ڵ�� ���� 100�̶� ���� �ѹ��� ������� �ʰ�, BonusValue�� ������.
        // cf) ���� �ڵ忡�� ���� ���� 100�� ���߾���.
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
        // ���� �������� ���� �ʱ� ������ ���� ����ġ ���ȸ�ŭ �÷��̾� ����ġ ����.
        // ���� ��ǥ(Target)�� �÷��̾��̱� ������ EntityBase Ÿ���� Target�� PlayerBase�� �� ��ȯ�ϰ�,
        // AccumulationExp ������Ƽ�� ���� �����ϰ� �ִ� ����ġ(Stats.CurrentExp.Value)�� ������.
        (Target as PlayerBase).AccumulationExp += Stats.CurrentExp.Value;

        // �� ����(this) ��� ó��.
        enemySpawner.Deactivate(this);
    }
}
