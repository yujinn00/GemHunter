public class ProjectileStraight : ProjectileBase
{
    public override void Setup(EntityBase target, float damage)
    {
        base.Setup(target, damage);

        // �߻�ü�� ��ǥ �������� ȸ��.
        transform.rotation = Utils.RotateToTarget(transform.position, target.MiddlePoint, 90);

        // �߻�ü �̵� ���� ����.
        movementRigidbody2D.MoveTo((target.MiddlePoint - transform.position).normalized);
    }

    public override void Process() { }
}
