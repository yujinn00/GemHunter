using UnityEngine;

public enum DestroyType { None = -1, Collision = 0, Indestructible, }
public enum AttackType { Single, Multiple }

public class ProjectileCollision2D : MonoBehaviour
{
    // �߻�ü�� �浹���� �� �ǰ� ȿ�� ������.
    [SerializeField]
    private Transform hitEffect;
    // �������� ����ϴ� Text UI ������.
    [SerializeField]
    private UIDamageText damageText;
    // �߻�ü�� �����Ǵ� ���.
    [SerializeField]
    private DestroyType destroyType = DestroyType.None;
    // �߻�ü�� ���� ����.
    [SerializeField]
    private AttackType attackType = AttackType.Single;
    // �߻�ü�� ���� �����ϴ��� ���θ� ��Ÿ���� bool ��.
    [SerializeField]
    private bool isIgnoreWall = false;

    // �߻�ü�� ��ǥ.
    private EntityBase target;
    // �߻�ü�� ���ݷ�.
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
