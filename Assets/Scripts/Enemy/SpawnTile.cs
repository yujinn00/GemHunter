using System.Collections;
using UnityEngine;

public class SpawnTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;  // 오브젝트의 알파 값 제어를 위한 변수.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 적의 등장 위치를 알려주는 타일은 생성 이후 활성/비활성해서 재활용하기 때문에,
    // 오브젝트를 활성화할 때 OnEnable() 메소드를 호출해 Fade 효과를 재생함.
    private void OnEnable()
    {
        StartCoroutine(nameof(FadeLoop));
    }

    // 오브젝트를 비활성화할 때 OnDisable() 메소드를 호출해 Fade 효과를 종료함.
    private void OnDisable()
    {
        StopCoroutine(nameof(FadeLoop));
    }

    private IEnumerator FadeLoop()
    {
        while (true)
        {
            yield return FadeEffect.Fade(spriteRenderer, 1, 0, 0.5f);
            yield return FadeEffect.Fade(spriteRenderer, 0, 1, 0.5f);
        }
    }
}
