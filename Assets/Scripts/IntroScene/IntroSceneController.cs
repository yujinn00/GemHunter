using System.Collections;
using UnityEngine;
using TMPro;

public class IntroSceneController : MonoBehaviour
{
    [SerializeField]
    private SceneNames nextScene;                   // 키를 입력했을 때 원하는 씬으로 전환하기 위한 다음 씬 정보.
    [SerializeField]
    private TextMeshProUGUI textPressAnyKey;        // "PRESS ANY KEY" Text UI 깜박임 제어를 위한 변수.

    private IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 1, 0));
            yield return StartCoroutine(FadeEffect.Fade(textPressAnyKey, 0, 1));
        }
    }

    private void Update()
    {
        if (Utils.IsAnyInputDown())
        {
            SceneLoader.Instance.LoadScene(nextScene);
        }
    }
}
