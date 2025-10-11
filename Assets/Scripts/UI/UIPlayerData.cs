using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textLevel;  // ���� ������ Text UI�� ����ϱ� ���� ����.
    [SerializeField]
    private Image fillGaugeEXP;         // ���� ����ġ�� Image UI�� ����ϱ� ���� ����.
    [SerializeField]
    private PlayerBase entity;          // ����, ����ġ ������ ������ �ִ� ����.

    private void Awake()
    {
        entity.Stats.CurrentExp.OnValueChanged += UpdateExp;
    }

    private void UpdateExp(Stat stat, float prev, float current)
    {
        // �÷��̾��� ���� ������ Text UI�� ���.
        textLevel.text = $"Lv.{entity.Stats.GetStat(StatType.Level).Value}";

        // �÷��̾��� ���� ����ġ�� Image UI�� ���.
        fillGaugeEXP.fillAmount = entity.Stats.CurrentExp.Value / entity.Stats.GetStat(StatType.Experience).Value;
    }
}
