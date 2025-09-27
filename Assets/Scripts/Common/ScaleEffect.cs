using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScaleEffect : MonoBehaviour
{
    [SerializeField]
    private float playTime = 0.1f;

    public void Play(Vector3 start, Vector3 end, UnityAction action=null)
    {
        StartCoroutine(ScaleAnimation(start, end, action));
    }

    private IEnumerator ScaleAnimation(Vector3 start, Vector3 end, UnityAction action)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / playTime;

            transform.localScale = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        action?.Invoke();
    }
}
