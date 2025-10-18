using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private ChapterData[] chapters;             // Lobby 씬에서 선택한 챕터의 정보를 불러오기 위해 모든 챕터 정보를 배열로 저장.
    [SerializeField]
    private EnemySpawner enemySpawner;          // 원하는 숫자만큼 적 생성을 제어하기 위한 변수.
    [SerializeField]
    private TextMeshPro textStageNumber;        // 스테이지가 바뀔 때 필드 바닥에 출력되는 스테이지 Text UI 갱신을 위한 변수.
    [SerializeField]
    private float enemyCountScale = 0.15f;      // 스테이지가 증가할 때 생성할 적 숫자 연산에 사용할 변수.

    private int currentChapter;                 // 현재 챕터 숫자.
    private int maxStage;                       // 현재 챕터의 최대 스테이지 숫자.
    private int currentStage = 0;               // 현재 스테이지 숫자.
    private int baseEnemyCount = 10;            // 생성할 적 기본 숫자.

    private void Start()
    {
        // Lobby 씬에서 저장한 선택 챕터 정보를 불러와 currentChapter에 저장.
        currentChapter = PlayerPrefs.GetInt(Constants.ChapterIndex);

        // 현재 챕터의 StageDataTable.maxStage 정보를 maxStage 변수에 저장.
        maxStage = chapters[currentChapter].StageDataTable.maxStage;

        // 현재 스테이지에 남아있는 적 숫자가 0이면 다음 스테이지로 넘어가도록 설정.
        EnemySpawner.exitEvent.AddListener(SetupStage);

        // 스테이지 설정, 스테이지에 등장하는 적 생성.
        SetupStage();
    }

    public void SetupStage()
    {
        // 현재 스테이지 숫자를 1만큼 증가.
        currentStage++;

        // 마지막 스테이지를 클리어했으면, Console View에 텍스트를 출력하고, 메소드를 종료.
        if (currentStage > maxStage)
        {
            Logger.Log("챕터 클리어");
            return;
        }

        // 맵에 출력하는 currentStage Text UI 갱신.
        textStageNumber.text = $"STAGE {currentStage:D2}";

        // 스테이지에 따라 등장하는 적 숫자 연산/생성.
        enemySpawner.SpawnEnemies((int)(baseEnemyCount + currentStage * enemyCountScale));
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
