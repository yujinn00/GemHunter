using UnityEngine;
using UnityEngine.EventSystems;

// IPointerDownHandler: 가상 조이스틱을 터치했을 때 반응하는 인터페이스.
// IDragHandler: 가상 조이스틱을 드래그했을 때 반응하는 인터페이스.
// IPointerUpHandler: 가상 조이스택을 터치 해제했을 때 반응하는 인터페이스.
public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private RectTransform rectBackground;       // 가상 조이스틱의 배경 이미지.
    [SerializeField]
    private RectTransform rectController;       // 가상 조이스틱의 컨트롤러 이미지.

    private Vector2 touchPosition;              // 터치/드래그 방향 정보.

    // 외부에서 조이스틱 드래그 방향 정보를 열람할 수 있도록 Get만 가능한 프로퍼티 정의.
    public float Horizontal => touchPosition.x;
    public float Vertical => touchPosition.y;

    private void Awake()
    {
        // 화면을 터치했을 때만 조이스틱이 화면에 보이도록 설정함.
        // 만약 계속 화면에 출력되길 원하면 이 코드를 삭제함.
        rectBackground.gameObject.SetActive(false);
    }

    /// <summary>
    /// 해당 오브젝트를 터치하는 순간 1회 호출되는 메소드.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        rectBackground.gameObject.SetActive(true);

        // 터치한 위치로 가상 컨트롤러 위치를 변경함.
        rectBackground.transform.position = eventData.position;
    }

    /// <summary>
    /// 해당 오브젝트를 터치한 상태에서 드래그할 때 매 프레임 호출되는 메소드.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        touchPosition = Vector2.zero;

        // 화면을 터치한 위치(eventData.position)가 Background 오브젝트(rectBackground)로부터,
        // 얼마나 떨어져 있는지 계산해(rectBackground의 Pivot, 위치 기준) touchPosition에 저장함.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectBackground, eventData.position, eventData.pressEventCamera, out touchPosition))
        {
            // 과정 1: touchPosition 값 연산.
            // touchPosition을 가상 컨트롤러 배경(rectBackground) 크기로 나누고 2를 곱해줌.
            touchPosition.x = (touchPosition.x / rectBackground.sizeDelta.x * 2);
            touchPosition.y = (touchPosition.y / rectBackground.sizeDelta.y * 2);

            // 과정 2: touchPosition 값의 정규화 [-1 ~ 1].
            // 현재 터치 위치가 가상 컨트롤러 배경(rectBackground) 바깥일 때,
            // -1 ~ 1보다 큰 값이 나오기 때문에 normailzed를 이용해 -1 ~ 1사이의 값으로 정규화함.
            touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;

            // 플레이어에게 보여주기 위해 실제 가상 조이스틱의 컨트롤러(rectController) 이미지 이동을 제어함.
            rectController.anchoredPosition = new Vector2(
                touchPosition.x * rectBackground.sizeDelta.x * 0.5f,
                touchPosition.y * rectBackground.sizeDelta.y * 0.5f);
        }
    }

    /// <summary>
    /// 해당 오브젝트 터치가 종료되는 순간 1회 호출되는 메소드.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        // 터치 종료 시 이미지의 위치를 중앙으로 이동시킴.
        rectController.anchoredPosition = Vector2.zero;
        // 터치 종료 시 touchPosition 값도 (0, 0)으로 초기화함.
        touchPosition = Vector2.zero;

        rectBackground.gameObject.SetActive(false);
    }
}
