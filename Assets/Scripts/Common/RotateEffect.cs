using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    // ȸ�� �ӵ�.
    [SerializeField]
    private float rotateSpeed = 100;
    // ȸ�� ����.
    [SerializeField]
    private bool isPlay = true;

    private void Update()
    {
        if (!isPlay)
        {
            return;
        }

        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);
    }
}
