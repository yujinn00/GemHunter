using UnityEngine;

public class PlayerBase : EntityBase
{
    [SerializeField]
    private FollowTarget targetMark;
    [SerializeField]
    private LevelData levelData;        // �ΰ��� ������ ����ġ ���̺� ����.
    [SerializeField]
    private SkillSystem skillSystem;    // ������ �� �� ��ų ���� UI ��� ����.

    private float expAmount = 2f;       // �� ������ ����ϴ� ����ġ ��.

    // ��ų ��� �� ���� ������ ����ϱ� ������ public ������Ƽ�� ����.
    // ���� �÷��̾ �̵� ������.
    public bool IsMoved { get; set; } = false;
    // ���� ���̰� ������ ����ġ.
    public float AccumulationExp { get; set; } = 0f;

    private void Awake()
    {
        base.Setup();

        // ���� ����ġ�� 0���� ����.
        Stats.CurrentExp.DefaultValue = 0f;
        // ���� ����ġ ���� ����� ������ IsLevelUp() �޼ҵ带 ȣ���ϵ��� OnValueChanged �̺�Ʈ�� ���.
        Stats.CurrentExp.OnValueChanged += IsLevelUp;
        // �������� �ʿ��� �ִ� ����ġ�� levelData.MaxExperience[0]���� ����.
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
        Logger.Log("�÷��̾� ��� ó��");
    }

    private void SearchTarget()
    {
        float closestDistSqr = Mathf.Infinity;

        foreach (var entity in EnemySpawner.Enemies)
        {
            // ���� ����� ����� ã�� ���� sqrMagnitude ���.
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
        // ü�� ȸ��.
        if (Stats.CurrentHP.DefaultValue < Stats.GetStat(StatType.HP).Value)
        {
            Stats.CurrentHP.DefaultValue += Time.deltaTime * Stats.GetStat(StatType.HPRecovery).Value;
        }
        else
        {
            Stats.CurrentHP.DefaultValue = Stats.GetStat(StatType.HP).Value;
        }
    }

    // ����ġ �������� ���������� ä�������� ���� ����� �� �÷��̾� ����ġ�� �ٷ� ������Ű�� �ʰ�,
    // ������ ������Ƽ(AccumulationExp)�� ����ġ�� ������ �� ������ ���ϴ� ��ġ(expAmount)��ŭ ����ġ�� ä�������� ��.
    private void UpdateExp()
    {
        // ������ ����ġ�� ���ų� ���� ��ų�� ���� ���̸� ����ġ�� ä������ �ʵ��� ������.
        if (Mathf.Approximately(AccumulationExp, 0f) || skillSystem.IsSelectSkill == true)
        {
            return;
        }

        float getExp = AccumulationExp > expAmount ? expAmount : AccumulationExp;
        AccumulationExp -= getExp;                  // ������ ����ġ(AccumulationExp)���� getExp��ŭ �Ҹ�.
        Stats.CurrentExp.DefaultValue += getExp;    // ���� �÷��̾� ����ġ�� getExp��ŭ ����.
    }

    private void IsLevelUp(Stat stat, float prev, float current)
    {
        // ����ġ�� �ִ밡 �ƴϸ� ������.
        if (!Mathf.Approximately(Stats.CurrentExp.Value, Stats.GetStat(StatType.Experience).Value))
        {
            return;
        }

        // �÷��̾� ������ (����� �ִ� ������ ��, UI�� ����ϰų� ���� ����).
        Stats.GetStat(StatType.Level).DefaultValue++;

        // ���� ����ġ ���� (�������� ����� �縸ŭ ����).
        Stats.CurrentExp.DefaultValue -= Stats.GetStat(StatType.Experience).Value;

        // �ִ� ����ġ ����.
        if (Stats.GetStat(StatType.Level).Value < levelData.MaxExperience.Length)
        {
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[(int)Stats.GetStat(StatType.Level).Value - 1];
        }
        else
        {
            Stats.GetStat(StatType.Experience).DefaultValue = levelData.MaxExperience[levelData.MaxExperience.Length - 1];
        }

        // ������ �� �� ��ų�� ����� �� �ֵ��� ���� �˾� ���.
        skillSystem.StartSelectSkill();
    }
}
