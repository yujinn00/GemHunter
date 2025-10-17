using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChapterIcon : MonoBehaviour
{
    [SerializeField]
    private GameObject lockedIcon;              // 챕터의 해금 여부에 따라 자물쇠 이미지의 활성 및 비활성을 제어하는 변수.
    [SerializeField]
    private Image imageChapter;                 // 챕터에 따라 출력되는 이미지를 제어하는 변수.
    [SerializeField]
    private TextMeshProUGUI textChapterName;    // 챕터에 따라 출력되는 이름을 제어하는 변수.
    [SerializeField]
    private TextMeshProUGUI textStage;          // 현재 챕터에서 도달한 스테이지와 최대 스테이지 출력을 제어하는 변수.

    public void Setup(int index, ChapterData chapterData)
    {
        // 매개변수로 받아온 현재 챕터 정보를 바탕으로 현재 챕터가 잠겨있으면 자물쇠 이미지를 활성화하고, 잠겨있지 않으면 비활성화함.
        lockedIcon.SetActive(!chapterData.ChapterDatabase.isUnlock);

        // 챕터 아이콘 이미지를 현재 챕터의 스프라이트로 설정함.
        imageChapter.sprite = chapterData.ChapterDataTable.spriteChapter;

        // Debug.. 챕터 이미지가 모두 있을 땐 색상 정보는 필요 없음.
        imageChapter.color = chapterData.ChapterDataTable.colorChapter;
        textChapterName.text = $"#{index+1:D2} {chapterData.ChapterDataTable.chapterName}";
        textStage.text = $"스테이지 {chapterData.ChapterDatabase.bestStage}/{chapterData.StageDataTable.maxStage}";
    }
}
