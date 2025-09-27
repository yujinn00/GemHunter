using System.Linq; // 배열, 리스트 등 컬렉션 데이터의 정렬 및 변환 등에 대한 메소드 제공.
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Level, Exp")]
    public int level;                   // 레벨.
    public float exp;                   // 경험치.

    // 현재 체력과 최대 체력은 동일하게 StatType.HP를 사용하기 때문에,
    // 현재 체력을 관리하는 별도의 currentHP 스탯 변수를 선언함.
    [Header("Current Stats")]
    [SerializeField]
    private Stat currentHP;

    // 그 외에 공격력, 쿨타임 등과 같은 기본 스탯 정보들은 stats 배열에 추가해서 사용함.
    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    // 외부에서 currentHP 변수 정보를 열람할 수 있도록 CurrentHP 프로퍼티를 정의함.
    public readonly Stat CurrentHP => currentHP;
    // 외부에서 stats 배열의 원하는 스탯 정보에 접근할 때는 GetStat() 메소드를 호출하고,
    // stats 배열에서 FirstOrDefault() 메소드를 호출해 매개변수 Stat 또는 StatType과 동일한 값의 스탯 정보를 반환함.
    public readonly Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}
