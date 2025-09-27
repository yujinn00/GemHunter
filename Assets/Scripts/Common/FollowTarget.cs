using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private bool x, y, z;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        // Ȱ��ȭ �� ���� target�� ��ġ, ��Ȱ��ȭ �� ���� �ڱ� �ڽ��� ��ġ�� ����.
        transform.position = new Vector3(
            (x ? target.position.x : transform.position.x),
            (y ? target.position.y : transform.position.y),
            (z ? target.position.z : transform.position.z)
        );
    }
}
