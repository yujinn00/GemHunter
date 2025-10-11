using UnityEngine;

// 인게임에서 최대 레벨, 각 레벨별 필요 경험치 정보를 에셋으로 저장하기 위해 ScriptableObject 클래스를 상속받음.
// Project View의 생성("+") 메뉴에 등록하기 위해 클래스 상단에 [CreateAssetMenu] 어트리뷰트를 작성함.
[CreateAssetMenu]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int maxLevel;               // 플레이어가 도달할 수 있는 최대 레벨.
    [SerializeField]
    private float[] maxExperience;      // 각 레벨에서 레벨업 할 때 필요한 경험치 배열.

    public int MaxLevel => maxLevel;
    public float[] MaxExperience => maxExperience;
}
