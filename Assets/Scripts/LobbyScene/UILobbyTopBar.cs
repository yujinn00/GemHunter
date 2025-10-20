using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobbyTopBar : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;                // Lobby 씬에서 사용하는 플레이어의 레벨/경험치 정보를 불러올 변수.
    [SerializeField]
    private TextMeshProUGUI textLevel;          // 플레이어의 레벨을 Text UI에 출력하기 위한 변수.
    [SerializeField]
    private Slider fillGaugeEXP;                // 플레이어의 경험치를 Slider UI에 출력하기 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textHeart;          // 플레이어의 하트 개수를 Text UI에 출력하기 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textHeartTimer;     // 하트 충전 시간을 Text UI에 출력하기 위한 변수.
    [SerializeField]
    private TextMeshProUGUI textGEMCount;       // 플레이어의 보석 개수를 Text UI에 출력하기 위한 변수.

    private void Awake()
    {
        // 현재는 로비에서 재화를 사용하지 않기 때문에 Lobby 씬을 로드할 때 1회만 호출함.
        // 일반적으로는 Stat.cs과 같이 재화에 Delegate, Event를 설정하고,
        // 값이 변경될 때마다 호출하도록 설정해서 사용해야 함.

        // 레벨과 경험치를 계산하고 출력함.
        UpdateLevel();
        // 보유 GEM을 포맷팅에 맞게 출력함.
        textGEMCount.text = NotateNumber.Transform((long)Database.DBItem.goods.gem);
    }

    private void UpdateLevel()
    {
        int level = Database.DBItem.player.level;

        // 경험치가 최대이면 레벨업함.
        while (Database.DBItem.player.experience >= levelData.MaxExperience[level - 1])
        {
            // 현재 LevelData에 레벨별 최대 경험치 배열의 크기는 10으로 되어 있음.
            // 즉, 10레벨까지는 테이블에 있는 값을 사용할 수 있고,
            // 그 이상부터는 마지막 레벨인 10레벨 테이블 값을 사용하도록 설정함.
            if (level > levelData.MaxExperience.Length)
            {
                level = levelData.MaxExperience.Length;
            }

            // 방금 레벨업에 소모한 경험치만큼 감소시킴.
            Database.DBItem.player.experience -= levelData.MaxExperience[level - 1];
            // 현재 레벨을 1 증가시킴.
            Database.DBItem.player.level++;
        }

        // 레벨과 경험치 정보가 변경되었기 때문에 파일에 데이터를 저장함.
        Database.Write();

        // Lobby 씬 화면에 레벨과 경험치 UI를 출력함.
        fillGaugeEXP.value = Database.DBItem.player.experience / levelData.MaxExperience[level - 1];
        textLevel.text = Database.DBItem.player.level.ToString();
    }

    public void UpdateHeart(int current, int max)
    {
        // 매개변수로 받아온 현재 하트 개수, 최대 하트 개수 정보를 Text UI에 출력함.
        textHeart.text = $"{current}/{max}";
    }

    public void UpdateHeartTimer(string text)
    {
        // 매개변수로 받아온 하트 타이머 정보를 Text UI에 출력함
        textHeartTimer.text = text;
    }
}
