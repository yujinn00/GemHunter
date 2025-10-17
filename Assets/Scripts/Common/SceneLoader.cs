using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// 씬 이름을 열거형으로 저장.
public enum SceneNames { Intro = 0, Lobby, Game }

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField]
    private GameObject loadingScreen;       // 로딩 화면.
    [SerializeField]
    private Image loadingBackground;        // 로딩 화면에 출력되는 배경 이미지.
    [SerializeField]
    private Sprite[] loadingSprites;        // 교체할 배경 이미지 목록.
    [SerializeField]
    private Slider loadingProgress;         // 로딩 진행도.
    [SerializeField]
    private TextMeshProUGUI textProgress;   // 로딩 진행도 텍스트.

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // SceneLoader가 또 생성되었으면 하나만 존재할 수 있도록 오브젝트를 삭제함.
            Destroy(gameObject);
        }
        else
        {
            // Instance에 현재 클래스 정보를 저장함.
            Instance = this;

            // 로딩 화면을 출력하는 LoadingCanvas 오브젝트가 씬이 전환되어도 삭제되지 않도록 설정함.
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string name)
    {
        // 배경 이미지 목록 중 임의의 배경 이미지 순번을 뽑아 index에 저장함.
        int index = Random.Range(0, loadingSprites.Length);
        // 로딩 화면에 출력되는 배경 이미지를 index번째 배경 이미지로 설정함.
        loadingBackground.sprite = loadingSprites[index];
        // 로딩 진행도를 0%로 설정함.
        loadingProgress.value = 0f;
        // 로딩 화면을 활성화함.
        loadingScreen.SetActive(true);

        // 비동기로 씬을 로드하는 코루틴 메소드를 실행함.
        StartCoroutine(LoadSceneAsync(name));
    }

    public void LoadScene(SceneNames name)
    {
        // 매개변수로 받아온 열거형 변수 name을 문자열로 변환해 LoadScene() 메소드를 호출함.
        LoadScene(name.ToString());
    }

    private IEnumerator LoadSceneAsync(string name)
    {
        // SceneManager.LoadSceneAsync() 메소드를 호출해 비동기로 씬을 불러오고,
        // 불러오는 name 씬의 작업 상태를 asyncOperation 변수에 저장함.
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);

        // 비동기 작업(씬 불러오기)이 완료될 때까지 반복.
        while (asyncOperation.isDone == false)
        {
            // 비동기 작업의 진행 상황 (0.0 ~ 1.0).
            loadingProgress.value = asyncOperation.progress;
            textProgress.text = $"{Mathf.RoundToInt(asyncOperation.progress * 100)}%";

            yield return null;
        }

        // 0.5초만큼 대기함.
        float changeDelay = 0.5f;
        yield return new WaitForSeconds(changeDelay);

        // 로딩 화면을 비활성화함.
        loadingScreen.SetActive(false);
    }
}
