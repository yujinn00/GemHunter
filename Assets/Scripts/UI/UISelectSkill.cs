using UnityEngine;

public class UISelectSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject selectSkillPanel;        // 스킬 선택 팝업 UI 활성 및 비활성 제어를 위한 변수.
    [SerializeField]
    private UISelectSkillIcon[] skillIcons;     // 스킬 선택 팝업 UI에 출력하는 3개의 스킬 정보 UI 배열.

    // 습득 or 레벨업 할 스킬을 선택할 때 호출하는 메소드.
    public void StartSelectSkillUI(SkillSystem system, SkillBase[] skills)
    {
        selectSkillPanel.SetActive(true);

        for (int i = 0; i < skillIcons.Length; ++i)
        {
            skillIcons[i].Setup(system, skills[i]);
        }
    }

    // 스킬 선택이 완료되었을 때 호출하는 메소드.
    public void EndSelectSkillUI()
    {
        selectSkillPanel.SetActive(false);
    }
}
