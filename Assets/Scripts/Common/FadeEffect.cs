using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class FadeEffect
{
    // 알파 값을 변경할 대상: SpriteRenderer, Image 등.
    // 알파의 시작 값: start.
    // 알파의 종료 값: end.
    // 페이드 효과를 재생하는 시간: fadeTime.
    public static IEnumerator Fade(SpriteRenderer target, float start, float end, float fadeTime=1f, UnityAction action=null)
    {
        if (target == null)
        {
            yield break;
        }

        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime / fadeTime;

            Color color = target.color;
            color.a = Mathf.Lerp(start, end, percent);
            target.color = color;

            yield return null;
        }

        // 페이드 효과 재생이 완료되면 action에 메소드가 들어있는지 null 검사를 진행하고,
        // null이 아니면 해당 메소드를 실행함.
        action?.Invoke();
    }
}
