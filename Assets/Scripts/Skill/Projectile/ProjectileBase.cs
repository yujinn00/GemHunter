using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected MovementRigidbody2D movementRigidbody2D;

    public virtual void Setup(SkillBase skillBase, float damage) { }

    public virtual void Setup(EntityBase target, float damage)
    {
        movementRigidbody2D = GetComponent<MovementRigidbody2D>();

        // �߻�ü�� ũ�⸦ 20%���� 100%���� Ȯ��.
        GetComponent<ScaleEffect>().Play(transform.localScale * 0.2f, transform.localScale);

        // �� ������Ʈ�� �浹 ó��.
        GetComponent<ProjectileCollision2D>().Setup(target, damage);
    }

    public virtual void Setup(EntityBase target, float damage, int maxCount, int index)
    {
        Setup(target, damage);
    }

    private void Update()
    {
        Process();
    }

    public abstract void Process();
}
