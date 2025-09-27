using UnityEngine;

[RequireComponent(typeof(MovementRigidbody2D))]
public class EnemyProjectile : MonoBehaviour
{
    private MovementRigidbody2D movementRigidbody2D;
    private ScaleEffect scaleEffect;
    private float damage;

    public void Setup(Vector3 target, float damage)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();
        scaleEffect = GetComponent<ScaleEffect>();
        this.damage = damage;

        // �߻�ü�� ũ�⸦ 20%���� 100%���� Ȯ��.
        scaleEffect.Play(transform.localScale * 0.2f, transform.localScale);

        // �߻�ü �̵� ���� ����.
        movementRigidbody2D.MoveTo((target - transform.position).normalized);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Player") && collision.TryGetComponent<EntityBase>(out var entity))
        {
            entity.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
