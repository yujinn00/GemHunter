using UnityEngine;

// 해당 스크립트를 컴포넌트로 추가할 때 Rigidbody2D 컴포넌트를 함께 추가함.
[RequireComponent(typeof(Rigidbody2D))]
public class MovementRigidbody2D : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Rigidbody2D rigid2D;

    private void Awake()
    {
        // Rigidbody2D 컴포넌트 정보를 얻어와 변수에 저장함.
        rigid2D = GetComponent<Rigidbody2D>();
    }

    public void MoveTo(Vector3 direction)
    {
        // 매개변수로 받아온 방향과 이동 속도를 곱해 플레이어의 속력으로 설정함.
        rigid2D.linearVelocity = direction * moveSpeed;
    }
}
