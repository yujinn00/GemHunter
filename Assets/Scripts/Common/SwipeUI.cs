using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SwipeUI : MonoBehaviour
{
    [SerializeField]
    private Scrollbar scrollbar;            // 스크롤바의 위치를 바탕으로 현재 페이지 검사.
    [SerializeField]
    private float swipeTime = 0.2f;         // 페이지가 스와이프되는 시간.
    [SerializeField]
    private float swipeDistance = 50.0f;    // 페이지가 스와이프되기 위해 움직여야 하는 최소 거리.

    private float[] scrollPageValues;       // 각 페이지의 위치 값 [0.0 ~ 1.0].
    private float valueDistance = 0;        // 각 페이지 사이의 거리.
    private int currentPage = 0;            // 현재 페이지.
    private int maxPage = 0;                // 최대 페이지.
    private float startTouchX;              // 터치 시작 위치.
    private float endTouchX;                // 터치 종료 위치.
    private bool isSwipeMode = false;       // 현재 스와이프가 되고 있는지 체크.

    // 현재 페이지 인덱스 정보.
    public int CurrentPage => currentPage;

    private void Start()
    {
        // 최대 페이지 수.
        maxPage = transform.childCount;
        // 스크롤되는 페이지의 각 value 값을 저장하는 배열 메모리 할당.
        scrollPageValues = new float[transform.childCount];
        // 스크롤되는 페이지 사이의 거리.
        valueDistance = 1f / (scrollPageValues.Length - 1f);

        // 스크롤되는 페이지의 각 value 위치 설정 [0 <= value <= 1].
        for (int i = 0; i < scrollPageValues.Length; ++i)
        {
            scrollPageValues[i] = valueDistance * i;
        }

        // 최초 시작할 때 0번 페이지를 볼 수 있도록 설정.
        SetScrollbarValue(0);
    }

    private void Update()
    {
        UpdateInput();
    }

    public void SetScrollbarValue(int index)
    {
        currentPage = index;
        scrollbar.value = scrollPageValues[index];
    }

    private void UpdateInput()
    {
        // 현재 스와이프를 진행 중이면 터치 불가.
        if (isSwipeMode == true)
        {
            return;
        }

        #if UNITY_EDITOR
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                startTouchX = Mouse.current.position.ReadValue().x;     // 클릭 시작 지점.
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                endTouchX = Mouse.current.position.ReadValue().x;       // 클릭 종료 지점.
                UpdateSwipe();
            }
        }
        #elif UNITY_ANDROID
        if (Touchscreen.current != null && Touchscreen.current.touches.Count > 0)
        {
            var touch = Touchscreen.current.touches[0];
            if (touch.press.wasPressedThisFrame)
            {
                startTouchX = touch.position.ReadValue().x;             // 터치 시작 지점 (스와이프 방향 구분).
            }
            else if (touch.press.wasReleasedThisFrame)
            {
                endTouchX = touch.position.ReadValue().x;               // 터치 종료 지점 (스와이프 방향 구분).
                UpdateSwipe();
            }
        }
        #endif
    }

    private void UpdateSwipe()
    {
        // 너무 작은 거리를 움직였을 때는 스와이프 취소.
        if (Mathf.Abs(startTouchX - endTouchX) < swipeDistance)
        {
            // 원래 페이지로 스와이프해서 돌아감.
            StartCoroutine(OnSwipeOneStep(currentPage));
            return;
        }

        // 스와이프 방향.
        bool isLeft = startTouchX < endTouchX;
        if (isLeft == true)                     // 이동 방향이 왼쪽일 때.
        {
            if (currentPage == 0)               // 현재 페이지가 왼쪽 끝이면 종료.
            {
                return;
            }

            currentPage--;                      // 왼쪽으로 이동을 위해 현재 페이지를 1만큼 감소.
        }
        else                                    // 이동 방향이 오른쪽일 때.
        {
            if (currentPage == maxPage - 1)     // 현재 페이지가 오른쪽 끝이면 종료.
            {
                return;
            }

            currentPage++;                      // 오른쪽으로 이동을 위해 현재 페이지를 1만큼 증가.
        }

        // currentIndex번째 페이지로 스와이프해서 이동.
        StartCoroutine(OnSwipeOneStep(currentPage));
    }

    /// <summary>
    /// 페이지를 한 장 옆으로 넘기는 스와이프 효과 재생.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator OnSwipeOneStep(int index)
    {
        // 스크롤을 시작하는 위치는 현재 위치로 설정함.
        float start = scrollbar.value;
        float percent = 0;

        // 스크롤 애니메이션을 재생하는 동안은 스크롤 행동을 할 수 없도록 플래그를 설정함.
        isSwipeMode = true;

        // 반복문은 스와이프 시간동안 매 프레임 호출함.
        while (percent < 1)
        {
            percent += Time.deltaTime / swipeTime;

            // start 위치에서 scrollPageValues[index] 위치까지 스와이프 시간동안 화면을 스크롤해서 이동함.
            scrollbar.value = Mathf.Lerp(start, scrollPageValues[index], percent);

            yield return null;
        }

        // 다시 스크롤을 할 수 있도록 플래그를 해제함.
        isSwipeMode = false;
    }
}
