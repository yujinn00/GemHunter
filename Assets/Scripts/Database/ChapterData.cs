using UnityEngine;

[CreateAssetMenu]
public class ChapterData : ScriptableObject
{
    // 2개의 커스텀 구조체 선언.
    [SerializeField]
    private ChapterDataTable chapterDataTable;
    [SerializeField]
    private StageDataTable stageDataTable;

    // 외부에서 변수에 접근할 수 있도록 Get만 가능한 프로퍼티 정의.
    public ChapterDataTable ChapterDataTable => chapterDataTable;
    public StageDataTable StageDataTable => stageDataTable;
}

// 해당 챕터의 배경 이미지, 색상, 이름을 저장하는 구조체.
[System.Serializable]
public struct ChapterDataTable
{
    public Sprite spriteChapter;        // 챕터 배경 이미지.
    public Color colorChapter;          // Debug.. 현재는 챕터 이미지가 없어서 색상 변경.
    public string chapterName;          // 챕터 이름.
}

// 해당 챕터의 최대 스테이지, 기본으로 등장하는 적의 숫자, 적의 레벨, 적의 프리팹 배열을 저장하는 구조체.
[System.Serializable]
public struct StageDataTable
{
    public int maxStage;                // 최대 스테이지.
    public int baseEnemyCount;          // 기본 적 숫자.
    public int baseEnemyLevel;          // 기본 적 레벨.
    public GameObject[] enemyPrefabs;   // 등장하는 적 프리팹 배열.
}
