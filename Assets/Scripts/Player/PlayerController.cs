using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // �÷��̾� ĳ���� �̵� ��� ���� movement2D ������ ������.
    private MovementRigidbody2D movement2D;

    // PlayerRenderer Ÿ���� playerRenderer ������ ������.
    private PlayerRenderer playerRenderer;

    // PlayerBase Ÿ���� playerBase ������ ������.
    private PlayerBase playerBase;

    // ����Ű�� ������ �� Ű ���� �����ϱ� ���� moveInput ������ ������.
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        // MovementRigidbody2D ������Ʈ ������ �ҷ��� movement2D ������ ������.
        movement2D = GetComponent<MovementRigidbody2D>();

        // �ڽ� ������Ʈ�� PlayerBase�� �ִ� PlayerRenderer ������Ʈ ������ �ҷ��� playerRenderer ������ ������.
        playerRenderer = GetComponentInChildren<PlayerRenderer>();

        // PlayerBase ������Ʈ ������ �ҷ��� playerBase ������ ������.
        playerBase = GetComponent<PlayerBase>();
    }

    private void Update()
    {
        // �÷��̾��� �̵� ���θ� �˻���.
        bool isMoved = moveInput.x != 0 || moveInput.y != 0;

        // �÷��̾ �¿� ������.
        if (moveInput.x != 0) playerRenderer.SpriteFlipX(moveInput.x);

        // �÷��̾� �ִϸ��̼��� �����.
        playerRenderer.OnMovement(playerBase.IsMoved ? 1 : 0);

        // ���� ����Ʈ�� �����.
        playerRenderer.OnFootStepEffect(playerBase.IsMoved);

        // movement2D.MoveTo() �޼ҵ忡 x, y ���� ���� �Ѱ��ָ� ������Ʈ�� �ش� �������� �̵���.
        movement2D.MoveTo(moveInput);

        // ��ǥ �������� �÷��̾�/���� ȸ��.
        playerRenderer.LookRotation(playerBase);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 1. started: �Է� ���۵�.
        // 2. performed: �Է� ������ �����.
        // 3. canceled: �Է� ��ҵ� (Ű�� �� ��).
        // InputActions ������ Action�� ��ϵǾ� �ִ� Ű�� �����ų� �� �� ������.
        if (context.performed || context.canceled)
        {
            // moveInput ������ Vector2 ���� ������.
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
