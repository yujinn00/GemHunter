using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Buff = 0, Emission, Sustained, Global, }
public enum SkillElement { None = -1, Ice = 100, Fire, Wind, Light, Dark, Count = 5 }

// 클래스 상단에 CreateAssetMenu 어트리뷰트를 작성하면 Project View의 생성("+") 메뉴에 메뉴로 등록됨.
[CreateAssetMenu(fileName = "NewSkill", menuName = "SkillAsset", order = 0)]
// 부모 클래스로 ScriptableObject를 사용하면 해당 클래스를 에셋의 형태로 저장할 수 있음.
public class SkillTemplate : ScriptableObject
{
    [Header("공통")]
    public string skillName;            // 스킬 이름.
    public SkillType skillType;         // 스킬 타입.
    public SkillElement element;        // 스킬 속성.
    public int maxLevel;                // 스킬 최대 레벨.
    [TextArea(1, 30)]
    public string description;          // 스킬 설명.
    public Sprite disableIcon;          // 스킬 미습득 아이콘.
    public Sprite enableIcon;           // 스킬 습득 아이콘.

    [Header("버프 스킬")]
    public List<Stat> buffStatList;     // 버프 스킬.

    [Header("공격 스킬")]
    public List<Stat> attackBaseStats;  // 공격 스킬 스탯 정보.
    public List<Stat> attackBuffStats;  // 레벨업 시 추가 스탯.
    public GameObject projectile;       // 발사체 프리팹.
}
