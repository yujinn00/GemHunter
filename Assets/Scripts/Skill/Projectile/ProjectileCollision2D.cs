using UnityEngine;

public enum DestroyType { None = -1, Collision = 0, Indestructible, }
public enum AttackType { Single, Multiple }

public class ProjectileCollision2D : MonoBehaviour
{
    // 발사체가 충돌했을 때 피격 효과 프리팹.
    [SerializeField]
    private Transform hitEffect;
    // 데미지를 출력하는 Text UI 프리팹.
    [SerializeField]
    private UIDamageText damageText;
    // 발사체가 삭제되는 방식.
    [SerializeField]
    private DestroyType destroyType = DestroyType.None;
    // 발사체의 공격 유형.
    [SerializeField]
    private AttackType attackType = AttackType.Single;
    // 발사체가 벽을 무시하는지 여부를 나타내는 bool 값.
    [SerializeField]
    private bool isIgnoreWall = false;

    // 발사체의 목표.
    private EntityBase target;
    // 발사체의 공격력.
    private float damage;

    public void Setup(EntityBase target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    public void SetTarget(EntityBase target) => this.target = target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") && isIgnoreWall == false)
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy") && collision.TryGetComponent<EntityBase>(out var entity))
        {
            if (attackType == AttackType.Single && entity != target)
            {
                return;
            }

            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, Quaternion.identity);
            }

            if (damageText != null)
            {
                UIDamageText clone = Instantiate(damageText, collision.transform.position, Quaternion.identity);
                clone.Setup(damage.ToString("F0"), Color.white);
            }

            entity.TakeDamage(damage);

            if (destroyType == DestroyType.Collision)
            {
                Destroy(gameObject);
            }
        }
    }
}
