using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private StageData stageData;

    private void LateUpdate()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, stageData.CameraLimitMin.x, stageData.CameraLimitMax.x);
        position.y = Mathf.Clamp(position.y, stageData.CameraLimitMin.y, stageData.CameraLimitMax.y);

        transform.position = position;
    }
}
