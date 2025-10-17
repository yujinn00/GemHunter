using UnityEngine;

public class LobbySceneController : MonoBehaviour
{
    [SerializeField]
    private GameObject chapterIconPrefab;   // 챕터 이미지, 챕터 이름, 스테이지 정보를 출력하는 챕터 프리팹.
    [SerializeField]
    private Transform parentContent;        // 챕터 오브젝트의 부모 Transform.
    [SerializeField]
    private ChapterData[] allChapter;       // 챕터 오브젝트 생성을 위한 모든 챕터 정보를 담고 있는 배열.
    [SerializeField]
    private SwipeUI swipeUI;                // 현재 선택된 챕터 정보를 불러오기 위한 변수.

    private void Awake()
    {
        for (int i = 0; i < allChapter.Length; ++i)
        {
            GameObject icon = Instantiate(chapterIconPrefab, parentContent);
            icon.GetComponent<UIChapterIcon>().Setup(i, allChapter[i]);
        }
    }

    // 게임 시작을 눌렀을 때 호출하는 메소드.
    public void ButtonEvent_GameStart()
    {
        // 현재 페이지 정보를 불러와 저장함.
        int index = swipeUI.CurrentPage;

        // 지금 선택된 챕터가 잠겨있으면 실행함.
        if (allChapter[index].ChapterDatabase.isUnlock == false)
        {
            // Console View에 텍스트를 출력하고 반환함.
            Logger.Log("현재 잠겨있는 챕터입니다.");
            return;
        }

        // 챕터가 해금되어 있으면 선택된 챕터 정보를 Console View에 출력하고 Game 씬을 로드함.
        Logger.Log($"챕터 {index + 1}을(를) 시작합니다.");
        SceneLoader.Instance.LoadScene(SceneNames.Game);
    }
}
