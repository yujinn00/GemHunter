using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField]
    protected GameObject projectilePrefab;      // 적이 공격할 때 생성하는 발사체 프리팹.
    [SerializeField]
    protected Transform projectileSpawnPoint;   // 발사체를 생성하는 위치.

    private float currentCooldownTime = 0f;     // 공격 쿨타임 계산을 위한 현재 쿨타임.
    private float maxCooldownTime = 0f;         // 공격 쿨타임 계산을 위한 최대 쿨타임.
    private bool isSkillAvailable = true;       // 공격 가능 여부.
    protected float damage;                     // 적으로부터 전달 받는 무기의 공격력.
    protected EntityBase owner;                 // 무기의 소유주 (적).

    public void Setup(EntityBase owner)
    {
        this.owner = owner;
        damage = owner.Stats.GetStat(StatType.Damage).Value;
        maxCooldownTime = owner.Stats.GetStat(StatType.CooldownTime).Value;
    }

    private void Update()
    {
        if (isSkillAvailable == false && Time.time - currentCooldownTime > maxCooldownTime)
        {
            isSkillAvailable = true;
        }
    }

    public void TryAttack()
    {
        if (isSkillAvailable == true)
        {
            OnAttack();
            isSkillAvailable = false;
            currentCooldownTime = Time.time;
        }
    }

    public abstract void OnAttack();
}
