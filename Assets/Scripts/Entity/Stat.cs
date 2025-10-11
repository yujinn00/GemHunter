using UnityEngine;

// Stat 클래스는 공격력, 체력과 같은 하나의 스탯 데이터로 활용하고,
// Inspector View에서 스탯 속성과 최댓값, 최솟값 등을 설정할 수 있도록,
// Serializable 어트리뷰트를 이용해 클래스를 직렬화함.
[System.Serializable]
public class Stat
{
    // 스탯의 값이 바뀌거나 최댓값과 최솟값에 도달했을 때,
    // 외부의 메소드를 호출할 수 있도록 처리하기 위해 3개의 매개변수를 가지는 델리게이트를 정의함.
    public delegate void ValueChangedHandler(Stat stat, float prev, float current);
    public event ValueChangedHandler OnValueChanged;    // 값이 변경될 때.
    public event ValueChangedHandler OnValueMax;        // 최댓값에 도달했을 때.
    public event ValueChangedHandler OnValueMin;        // 최솟값에 도달했을 때.

    [SerializeField]
    private StatType statType;      // 현재 스탯이 어떤 스탯 정보인지 나타내는 변수.
    [SerializeField]
    private float maxValue;         // 현재 스탯이 가질 수 있는 최댓값.
    [SerializeField]
    private float minValue;         // 현재 스탯이 가질 수 있는 최솟값.
    [SerializeField]
    private float defaultValue;     // 현재 스탯의 기본 값.
    [SerializeField]
    private float bonusValue;       // 아이템, 스킬 등에 의해 추가로 증가하는 값.

    public void CopyData(Stat newStat)
    {
        statType = newStat.StatType;
        maxValue = newStat.MaxValue;
        minValue = newStat.MinValue;
        defaultValue = newStat.DefaultValue;
        bonusValue = newStat.BonusValue;
    }

    public StatType StatType => statType;
    public float MaxValue => maxValue;
    public float MinValue => minValue;
    public float Value => Mathf.Clamp(defaultValue + bonusValue, minValue, maxValue);

    // DefaultValue 프로퍼티는 기본 값을 사용하거나 수정할 때 호출함.
    public float DefaultValue
    {
        get => defaultValue;
        set
        {
            float prev = Value;
            defaultValue = Mathf.Clamp(value, minValue, maxValue);
            TryInvokeValueChangedEvent(prev, Value);
        }
    }

    // BonusValue 프로퍼티는 외부에서 get/set 할 수 있도록 정의함.
    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    
    private void TryInvokeValueChangedEvent(float prev, float current)
    {
        // TryInvokeValueChangedEvent() 메소드는 최종 값에 변화가 있을 때,
        if (!Mathf.Approximately(prev, current))
        {
            // OnValueChanged.Invoke() 메소드를 호출해, OnValueChanged 이벤트에 등록되어 있는 메소드들을 호출함.
            OnValueChanged?.Invoke(this, prev, current);

            // 최종 값이 최솟값과 동일하거나 최댓값과 동일할 때,
            // 각각 OnValueMin, OnValueMax 이벤트에 등록되어 있는 메소드들을 호출함.
            if (Mathf.Approximately(current, maxValue))
            {
                OnValueMax?.Invoke(this, prev, maxValue);
            }
            else if (Mathf.Approximately(current, minValue))
            {
                OnValueMin?.Invoke(this, prev, minValue);
            }
        }
    }
}

// StatType 열거형은 오브젝트들이 가질 수 있는 모든 스탯에 대한 정보임.
// 속성 공격력, 속성 저항 등 새로운 스탯을 추가하고 싶을 때,
// StatType 열거형에 변수를 추가하고, Inspector View에서 해당 스탯 정보를 설정함.
public enum StatType { Damage = 0, CooldownTime, CriticalChance, CriticalMultiplier, HP, Evasion,
                       MetastatisCount, HPRecovery, ProjectileCount, Duration, AttackRate,
                       Level, Experience,
                       IceElementalBonus = 100, FireElementalBonus, WindElementalBonus,
                       LightElementalBonus, DarkElementalBonus }
