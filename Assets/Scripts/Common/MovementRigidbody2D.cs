using UnityEngine;

// �ش� ��ũ��Ʈ�� ������Ʈ�� �߰��� �� Rigidbody2D ������Ʈ�� �Բ� �߰���.
[RequireComponent(typeof(Rigidbody2D))]
public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        // Rigidbody2D ������Ʈ ������ ���� ������ ������.
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        // �Ű������� �޾ƿ� ����� �̵� �ӵ��� ���� �÷��̾��� �ӷ����� ������.
        rigid2D.linearVelocity = direction * moveSpeed;
    }
}
