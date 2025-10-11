using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMark;
    [SerializeField]
    private LevelData levelData;        // 인게임 레벨별 경험치 테이블 정보.
    [SerializeField]
    private SkillSystem skillSystem;    // 레벨업 할 때 스킬 선택 UI 출력 제어.

    private float expAmount = 2f;       // 매 프레임 흡수하는 경험치 양.

    // 스킬 사용 등 여러 곳에서 사용하기 때문에 public 프로퍼티로 정의.
    // 현재 플레이어가 이동 중인지.
    public bool IsMoved { get; set; } = false;
    // 적을 죽이고 축적된 경험치.
    public float AccumulationExp { get; set; } = 0f;

    private void Awake()
    {
        base.Setup();

        // 현재 경험치를 0으로 설정.
        Stats.CurrentExp.DefaultValue = 0f;
        // 현재 경험치 값이 변경될 때마다 IsLevelUp() 메소드를 호출하도록 OnValueChanged 이벤트에 등록.
        Stats.CurrentExp.OnValueChanged += IsLevelUp;
        // 레벨업에 필요한 최대 경험치를 levelData.MaxExperience[0]으로 설정.
        Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[0];
    }

    private void Update()
    {
        if (Target == null)
        {
            targetMark.gameObject.SetActive(false);
        }

        SearchTarget();
        Recovery();
        UpdateExp();
    }

    protected override void OnDie()
    {
        Logger.Log("플레이어 사망 처리");
    }

    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach (var entity in EnemySpawner.Enemies)
        {
            // 제일 가까운 대상을 찾기 위해 sqrMagnitude 사용.
            float distance = (entity.transform.position - transform.position).sqrMagnitude;

            if (distance < closestDistSqr)
            {
                closestDistSqr = distance;
                Target = entity.GetComponent<EntityBase>();
            }
        }

        if (Target != null)
        {
            targetMark.SetTarget(Target.transform);
            targetMark.transform.position = Target.transform.position;
            targetMark.gameObject.SetActive(true);
        }
    }

    private void Recovery()
    {
        // 체력 회복.
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
        {
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        }
        else
        {
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
        }
    }

    // 경험치 게이지가 점진적으로 채워지도록 적이 사망할 때 플레이어 경험치를 바로 증가시키지 않고,
    // 별도의 프로퍼티(AccumulationExp)에 경험치를 축적해 매 프레임 원하는 수치(expAmount)만큼 경험치가 채워지도록 함.
    private void UpdateExp()
    {
        // 축적된 경험치가 없거나 현재 스킬을 선택 중이면 경험치가 채워지지 않도록 리턴함.
        if (Mathf.Approximately(AccumulationExp, 0f) || skillSystem.IsSelectSkill == true)
        {
            return;
        }

        float getExp = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        AccumulationExp -= getExp;                  // 축적된 경험치(AccumulationExp)에서 getExp만큼 소모.
        Stats.CurrentExp.DefaultValue += getExp;    // 실제 플레이어 경험치를 getExp만큼 증가.
    }

    private void IsLevelUp(Stat stat, float prev, float current)
    {
        // 경험치가 최대가 아니면 리턴함.
        if (!Mathf.Approximately(Stats.CurrentExp.Value, Stats.GetStat(StatType.Experience).Value))
        {
            return;
        }

        // 플레이어 레벨업 (현재는 최대 레벨일 때, UI를 출력하거나 하지 않음).
        Stats.GetStat(StatType.Level).DefaultValue++;

        // 현재 경험치 설정 (레벨업에 사용한 양만큼 감소).
        Stats.CurrentExp.DefaultValue -= Stats.GetStat(StatType.Experience).Value;

        // 최대 경험치 설정.
        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExperience.Length)
        {
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[(int)Stats.GetStat(StatType.Level).Value - 1];
        }
        else
        {
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[levelData.MaxExperience.Length - 1];
        }

        // 레벨업 할 때 스킬을 출력할 수 있도록 선택 팝업 출력.
        skillSystem.StartSelectSkill();
    }
}
