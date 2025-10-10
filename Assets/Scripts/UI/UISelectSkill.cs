using UnityEngine;

public class UISelectSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject selectSkillPanel;        // ��ų ���� �˾� UI Ȱ�� �� ��Ȱ�� ��� ���� ����.
    [SerializeField]
    private UISelectSkillIcon[] skillIcons;     // ��ų ���� �˾� UI�� ����ϴ� 3���� ��ų ���� UI �迭.

    // ���� or ������ �� ��ų�� ������ �� ȣ���ϴ� �޼ҵ�.
    public void StartSelectSkillUI(SkillSystem system, SkillBase[] skills)
    {
        selectSkillPanel.SetActive(true);

        for (int i = 0; i < skillIcons.Length; ++i)
        {
            skillIcons[i].Setup(system, skills[i]);
        }
    }

    // ��ų ������ �Ϸ�Ǿ��� �� ȣ���ϴ� �޼ҵ�.
    public void EndSelectSkillUI()
    {
        selectSkillPanel.SetActive(false);
    }
}
