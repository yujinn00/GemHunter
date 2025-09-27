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
        // �⺻ ü���� DefaultValue�� �ش��ϱ� ������,
        // ���� �ڵ�� ���� 100�̶� ���� �ѹ��� ������� �ʰ�, BonusValue�� ������.
        // cf) ���� �ڵ忡�� ���� ���� 100�� ���߾���.
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
