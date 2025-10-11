using System.Linq; // 배열, 리스트 등 컬렉션 데이터의 정렬 및 변환 등에 대한 메소드 제공.
using UnityEngine;

[System.Serializable]
public struct EntityStats
{
    [Header("Current Stats")]
    // 경험치는 현재 경험치, 최대 경험치로 나뉘기 때문에 체력과 마찬가지로,
    // 현재 경험치 스탯은 currentExp 변수를 별도로 선언하고,
    // 레벨과 최대 경험치 스탯은 stats 배열에서 관리하기 때문에 변수를 선언하지 않음.
    [SerializeField]
    private Stat currentExp;
    // 현재 체력과 최대 체력은 동일하게 StatType.HP를 사용하기 때문에,
    // 현재 체력을 관리하는 별도의 currentHP 스탯 변수를 선언함.
    [SerializeField]
    private Stat currentHP;

    // 그 외에 공격력, 쿨타임 등과 같은 기본 스탯 정보들은 stats 배열에 추가해서 사용함.
    [Header("Stats")]
    [SerializeField]
    private Stat[] stats;

    // 현재 경험치 정보를 외부에서 열람할 수 있도록 Get만 가능한 CurrentExp 프로퍼티를 정의함.
    public readonly Stat CurrentExp => currentExp;
    // 외부에서 currentHP 변수 정보를 열람할 수 있도록 CurrentHP 프로퍼티를 정의함.
    public readonly Stat CurrentHP => currentHP;
    // 외부에서 stats 배열의 원하는 스탯 정보에 접근할 때는 GetStat() 메소드를 호출하고,
    // stats 배열에서 FirstOrDefault() 메소드를 호출해 매개변수 Stat 또는 StatType과 동일한 값의 스탯 정보를 반환함.
    public readonly Stat GetStat(Stat stat) => stats.FirstOrDefault(s => s.StatType == stat.StatType);
    public readonly Stat GetStat(StatType statType) => stats.FirstOrDefault(s => s.StatType == statType);
}
