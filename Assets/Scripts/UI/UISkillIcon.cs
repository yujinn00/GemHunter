using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    [SerializeField]
    private Image skillIcon;                // ��ų�� �������� �� Image UI�� ��ų ������ �̹����� �����ϱ� ���� ����.
    [SerializeField]
    private TextMeshProUGUI skillLevel;     // ��ų�� ������ ������ �� ��ų ���� ���� ����� ���� ����.

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
