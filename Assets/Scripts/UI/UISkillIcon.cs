using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;                // 스킬을 습득했을 때 Image UI의 스킬 아이콘 이미지를 변경하기 위한 변수.
    [SerializeField]
    private TextMeshProUGUI skillLevel;     // 스킬의 레벨이 증가할 때 스킬 레벨 정보 출력을 위한 변수.

    public void Setup(Sprite defaultSprite)
    {
        skillIcon.sprite = defaultSprite;
        skillLevel.text = "-";
    }

    public void LevelUp(int currentLevel, Sprite activeSprite)
    {
        skillIcon.sprite = activeSprite;
        skillLevel.text = currentLevel.ToString();
    }
}
