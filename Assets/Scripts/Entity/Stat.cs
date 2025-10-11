using UnityEngine;

// Stat Ŭ������ ���ݷ�, ü�°� ���� �ϳ��� ���� �����ͷ� Ȱ���ϰ�,
// Inspector View���� ���� �Ӽ��� �ִ�, �ּڰ� ���� ������ �� �ֵ���,
// Serializable ��Ʈ����Ʈ�� �̿��� Ŭ������ ����ȭ��.
[System.Serializable]
public class Stat
{
    // ������ ���� �ٲ�ų� �ִ񰪰� �ּڰ��� �������� ��,
    // �ܺ��� �޼ҵ带 ȣ���� �� �ֵ��� ó���ϱ� ���� 3���� �Ű������� ������ ��������Ʈ�� ������.
    public delegate void ValueChangedHandler(Stat stat, float prev, float current);
    public event ValueChangedHandler OnValueChanged;    // ���� ����� ��.
    public event ValueChangedHandler OnValueMax;        // �ִ񰪿� �������� ��.
    public event ValueChangedHandler OnValueMin;        // �ּڰ��� �������� ��.

    [SerializeField]
    private StatType statType;      // ���� ������ � ���� �������� ��Ÿ���� ����.
    [SerializeField]
    private float maxValue;         // ���� ������ ���� �� �ִ� �ִ�.
    [SerializeField]
    private float minValue;         // ���� ������ ���� �� �ִ� �ּڰ�.
    [SerializeField]
    private float defaultValue;     // ���� ������ �⺻ ��.
    [SerializeField]
    private float bonusValue;       // ������, ��ų � ���� �߰��� �����ϴ� ��.

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

    // DefaultValue ������Ƽ�� �⺻ ���� ����ϰų� ������ �� ȣ����.
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

    // BonusValue ������Ƽ�� �ܺο��� get/set �� �� �ֵ��� ������.
    public float BonusValue
    {
        get => bonusValue;
        set => bonusValue = value;
    }

    
    private void TryInvokeValueChangedEvent(float prev, float current)
    {
        // TryInvokeValueChangedEvent() �޼ҵ�� ���� ���� ��ȭ�� ���� ��,
        if (!Mathf.Approximately(prev, current))
        {
            // OnValueChanged.Invoke() �޼ҵ带 ȣ����, OnValueChanged �̺�Ʈ�� ��ϵǾ� �ִ� �޼ҵ���� ȣ����.
            OnValueChanged?.Invoke(this, prev, current);

            // ���� ���� �ּڰ��� �����ϰų� �ִ񰪰� ������ ��,
            // ���� OnValueMin, OnValueMax �̺�Ʈ�� ��ϵǾ� �ִ� �޼ҵ���� ȣ����.
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

// StatType �������� ������Ʈ���� ���� �� �ִ� ��� ���ȿ� ���� ������.
// �Ӽ� ���ݷ�, �Ӽ� ���� �� ���ο� ������ �߰��ϰ� ���� ��,
// StatType �������� ������ �߰��ϰ�, Inspector View���� �ش� ���� ������ ������.
public enum StatType { Damage = 0, CooldownTime, CriticalChance, CriticalMultiplier, HP, Evasion,
                       MetastatisCount, HPRecovery, ProjectileCount, Duration, AttackRate,
                       Level, Experience,
                       IceElementalBonus = 100, FireElementalBonus, WindElementalBonus,
                       LightElementalBonus, DarkElementalBonus }
