using UnityEngine;
using TMPro;

public enum RewardType { GEM, EXP, ITEM }

public class UIRewardResult : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;  // 게임 속도 제어를 위한 변수.
    [SerializeField]
    private GameObject panelResult;         // 보상 결과 화면 활성/비활성 제어를 위한 변수.
    [SerializeField]
    private GameObject textNewRecord;       // "New Record" 텍스트 활성/비활성 제어를 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textTheme;      // "챕터 클리어", "게임 오버" 텍스트 출력 제어를 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textChapter;    // 챕터 숫자 출력을 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textStage;      // 스테이지 숫자 출력을 위한 변수.
    [SerializeField]
    private Transform rewardParent;         // 보상 아이콘이 배치될 부모 트랜스폼.
    [SerializeField]
    private UIRewardIcon[] rewards;         // 보상 아이콘을 생성할 프리팹 배열.

    // 챕터 클리어 또는 게임 오버 했을 때 결과 UI를 출력하는 메소드.
    public void OnRewardResult(bool isNewRecord, bool isClear, int chapter, int stage, (RewardType, long)[] items)
    {
        panelResult.SetActive(true);
        textNewRecord.SetActive(isNewRecord);

        if (isClear == true)
        {
            textTheme.text = "챕터 클리어";
        }
        else
        {
            textTheme.text = "게임 오버";
        }

        textChapter.text = $"CHAPTER {chapter + 1:D2}";
        textStage.text = stage.ToString();

        UIRewardIcon item;
        for (int i = 0; i < items.Length; ++i)
        {
            // 보상 아이콘 오브젝트 생성 (각각 GEM, EXP, ITEM).
            item = Instantiate(rewards[(int)items[i].Item1], rewardParent);

            // 보상에 출력되는 재화의 양 출력.
            item.SetReward(items[i].Item2);
        }
    }

    public void ButtonEvent_ReturnLobby()
    {
        gameController.SetTimeScale(1);
        SceneLoader.Instance.LoadScene(SceneNames.Lobby);
    }
}
