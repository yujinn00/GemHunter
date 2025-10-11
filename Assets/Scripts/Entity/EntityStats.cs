using System.Linq; // �迭, ����Ʈ �� �÷��� �������� ���� �� ��ȯ � ���� �޼ҵ� ����.
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Current Stats")]
    // ����ġ�� ���� ����ġ, �ִ� ����ġ�� ������ ������ ü�°� ����������,
    // ���� ����ġ ������ currentExp ������ ������ �����ϰ�,
    // ������ �ִ� ����ġ ������ stats �迭���� �����ϱ� ������ ������ �������� ����.
    [SerializeField]
    private Stat currentExp;
    // ���� ü�°� �ִ� ü���� �����ϰ� StatType.HP�� ����ϱ� ������,
    // ���� ü���� �����ϴ� ������ currentHP ���� ������ ������.
    [SerializeField]
    private Stat currentHP;

    // �� �ܿ� ���ݷ�, ��Ÿ�� ��� ���� �⺻ ���� �������� stats �迭�� �߰��ؼ� �����.
    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    // ���� ����ġ ������ �ܺο��� ������ �� �ֵ��� Get�� ������ CurrentExp ������Ƽ�� ������.
    public readonly Stat CurrentExp => currentExp;
    // �ܺο��� currentHP ���� ������ ������ �� �ֵ��� CurrentHP ������Ƽ�� ������.
    public readonly Stat CurrentHP => currentHP;
    // �ܺο��� stats �迭�� ���ϴ� ���� ������ ������ ���� GetStat() �޼ҵ带 ȣ���ϰ�,
    // stats �迭���� FirstOrDefault() �޼ҵ带 ȣ���� �Ű����� Stat �Ǵ� StatType�� ������ ���� ���� ������ ��ȯ��.
    public readonly Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}
