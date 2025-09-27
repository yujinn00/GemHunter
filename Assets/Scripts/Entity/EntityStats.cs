using System.Linq; // �迭, ����Ʈ �� �÷��� �������� ���� �� ��ȯ � ���� �޼ҵ� ����.
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;                   // ����.
    public float exp;                   // ����ġ.

    // ���� ü�°� �ִ� ü���� �����ϰ� StatType.HP�� ����ϱ� ������,
    // ���� ü���� �����ϴ� ������ currentHP ���� ������ ������.
    [Header("Current Stats")]
    [SerializeField]
    private Stat currentHP;

    // �� �ܿ� ���ݷ�, ��Ÿ�� ��� ���� �⺻ ���� �������� stats �迭�� �߰��ؼ� �����.
    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    // �ܺο��� currentHP ���� ������ ������ �� �ֵ��� CurrentHP ������Ƽ�� ������.
    public readonly Stat CurrentHP => currentHP;
    // �ܺο��� stats �迭�� ���ϴ� ���� ������ ������ ���� GetStat() �޼ҵ带 ȣ���ϰ�,
    // stats �迭���� FirstOrDefault() �޼ҵ带 ȣ���� �Ű����� Stat �Ǵ� StatType�� ������ ���� ���� ������ ��ȯ��.
    public readonly Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}
