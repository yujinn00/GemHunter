using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Buff = 0, Emission, Sustained, Global, }
public enum SkillElement { None = -1, Ice = 100, Fire, Wind, Light, Dark, Count = 5 }

// Ŭ���� ��ܿ� CreateAssetMenu ��Ʈ����Ʈ�� �ۼ��ϸ� Project View�� ����("+") �޴��� �޴��� ��ϵ�.
[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillAsset", order = 0)]
// �θ� Ŭ������ ScriptableObject�� ����ϸ� �ش� Ŭ������ ������ ���·� ������ �� ����.
public class SkillTemplate : ScriptableObject
{
    [Header("����")]
    public string skillName;            // ��ų �̸�.
    public SkillType skillType;         // ��ų Ÿ��.
    public SkillElement element;        // ��ų �Ӽ�.
    public int maxLevel;                // ��ų �ִ� ����.
    [TextArea(1, 30)]
    public string description;          // ��ų ����.
    public Sprite disableIcon;          // ��ų �̽��� ������.
    public Sprite enableIcon;           // ��ų ���� ������.

    [Header("���� ��ų")]
    public List<Stat> buffStatList;     // ���� ��ų.

    [Header("���� ��ų")]
    public List<Stat> attackBaseStats;  // ���� ��ų ���� ����.
    public List<Stat> attackBuffStats;  // ������ �� �߰� ����.
    public GameObject projectile;       // �߻�ü ������.
}
