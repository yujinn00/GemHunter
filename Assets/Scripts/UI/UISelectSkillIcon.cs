using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// UISelectSkillIcon 클래스는 IPointerClickHandler 인터페이스를 상속받고,
// UI 오브젝트를 클릭했을 때 호출되는 OnPointerClick() 메소드를 정의함.
public class UISelectSkillIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image skillicon;                    // 스킬 이미지 출력을 위한 변수.
    [SerializeField]
    private TextMeshProUGUI skillName;          // 스킬 이름 출력을 위한 변수.
    [SerializeField]
    private TextMeshProUGUI skillDescription;   // 스킬 설명 출력을 위한 변수.

    private SkillSystem skillSystem;            // 선택된 스킬 정보 전달을 위한 변수.
    private SkillBase skillBase;                // 스킬 정보를 위한 변수.

    public void Setup(SkillSystem skillSystem, SkillBase skillBase)
    {
        this.skillSystem = skillSystem;
        this.skillBase = skillBase;
        skillicon.sprite = skillBase.EnableIcon;
        skillName.text = skillBase.SkillName;
        skillDescription.text = skillBase.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillSystem.EndSelectSkill(skillBase);
    }
}
