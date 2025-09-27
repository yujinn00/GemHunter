using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private EntityBase entity;

    // 플레이어 체력 UI는 게임 시작 전에 Hierachy View에 미리 생성했기 때문에 Awake() 메소드에서 처리함.
    private void Awake()
    {
        if (entity != null)
        {
            entity.Stats.CurrentHP.OnValueChanged += UpdateHP;
        }
    }

    public void Setup(EntityBase entity)
    {
        this.entity = entity;

        // 적 체력 UI는 적이 생성될 때 생성하기 때문에 Setup() 메소드에서 처리함.
        this.entity.Stats.CurrentHP.OnValueChanged += UpdateHP;
    }

    private void UpdateHP(Stat stat, float prev, float current)
    {
        image.fillAmount = entity.Stats.CurrentHP.Value / entity.Stats.GetStat(StatType.HP).Value;
    }
}
