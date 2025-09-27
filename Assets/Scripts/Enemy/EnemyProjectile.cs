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

        // 발사체의 크기를 20%에서 100%까지 확대.
        scaleEffect.Play(transform.localScale * 0.2f, transform.localScale);

        // 발사체 이동 방향 설정.
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
