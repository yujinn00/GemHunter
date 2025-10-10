using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// UISelectSkillIcon Ŭ������ IPointerClickHandler �������̽��� ��ӹް�,
// UI ������Ʈ�� Ŭ������ �� ȣ��Ǵ� OnPointerClick() �޼ҵ带 ������.
public class UISelectSkillIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image skillicon;                    // ��ų �̹��� ����� ���� ����.
    [SerializeField]
    private TextMeshProUGUI skillName;          // ��ų �̸� ����� ���� ����.
    [SerializeField]
    private TextMeshProUGUI skillDescription;   // ��ų ���� ����� ���� ����.

    private SkillSystem skillSystem;            // ���õ� ��ų ���� ������ ���� ����.
    private SkillBase skillBase;                // ��ų ������ ���� ����.

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
