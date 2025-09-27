using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 플레이어 캐릭터 이동 제어를 위해 movement2D 변수를 선언함.
    private MovementRigidbody2D movement2D;

    // PlayerRenderer 타입의 playerRenderer 변수를 선언함.
    private PlayerRenderer playerRenderer;

    // PlayerBase 타입의 playerBase 변수를 선언함.
    private PlayerBase playerBase;

    // 방향키를 눌렀을 때 키 값을 저장하기 위해 moveInput 변수를 선언함.
    private Vector2 moveInput = Vector2.zero;

    private void Awake()
    {
        // MovementRigidbody2D 컴포넌트 정보를 불러와 movement2D 변수에 저장함.
        movement2D = GetComponent<MovementRigidbody2D>();

        // 자식 오브젝트인 PlayerBase에 있는 PlayerRenderer 컴포넌트 정보를 불러와 playerRenderer 변수에 저장함.
        playerRenderer = GetComponentInChildren<PlayerRenderer>();

        // PlayerBase 컴포넌트 정보를 불러와 playerBase 변수에 저장함.
        playerBase = GetComponent<PlayerBase>();
    }

    private void Update()
    {
        // 플레이어의 이동 여부를 검사함.
        bool isMoved = moveInput.x != 0 || moveInput.y != 0;

        // 플레이어를 좌우 반전함.
        if (moveInput.x != 0) playerRenderer.SpriteFlipX(moveInput.x);

        // 플레이어 애니메이션을 재생함.
        playerRenderer.OnMovement(playerBase.IsMoved ? 1 : 0);

        // 먼지 이펙트를 재생함.
        playerRenderer.OnFootStepEffect(playerBase.IsMoved);

        // movement2D.MoveTo() 메소드에 x, y 방향 값을 넘겨주면 오브젝트가 해당 방향으로 이동함.
        movement2D.MoveTo(moveInput);

        // 목표 방향으로 플레이어/무기 회전.
        playerRenderer.LookRotation(playerBase);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // 1. started: 입력 시작됨.
        // 2. performed: 입력 동작이 수행됨.
        // 3. canceled: 입력 취소됨 (키를 뗄 때).
        // InputActions 파일의 Action에 등록되어 있는 키를 누르거나 뗄 때 실행함.
        if (context.performed || context.canceled)
        {
            // moveInput 변수에 Vector2 값을 저장함.
            moveInput = context.ReadValue<Vector2>();
        }
    }
}
