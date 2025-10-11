using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPlayerData : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textLevel;  // 현재 레벨을 Text UI에 출력하기 위한 변수.
    [SerializeField]
    private Image fillGaugeEXP;         // 현재 경험치를 Image UI에 출력하기 위한 변수.
    [SerializeField]
    private PlayerBase entity;          // 레벨, 경험치 정보를 가지고 있는 변수.

    private void Awake()
    {
        entity.Stats.CurrentExp.OnValueChanged += UpdateExp;
    }

    private void UpdateExp(Stat stat, float prev, float current)
    {
        // 플레이어의 현재 레벨을 Text UI에 출력.
        textLevel.text = $"Lv.{entity.Stats.GetStat(StatType.Level).Value}";

        // 플레이어의 현재 경험치를 Image UI에 출력.
        fillGaugeEXP.fillAmount = entity.Stats.CurrentExp.Value / entity.Stats.GetStat(StatType.Experience).Value;
    }
}
