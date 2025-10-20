using UnityEngine;
using TMPro;

public class UIRewardIcon : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textReward;     // 보상의 양을 출력하는 변수.

    public void SetReward(long reward)
    {
        textReward.text = reward.ToString();
    }
}
