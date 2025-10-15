using UnityEngine;
using UnityEngine.Events;

public class GemCollector : MonoBehaviour
{
    [SerializeField]
    private GameObject gemEffectPrefab;         // GEM 이펙트 프리팹.
    [SerializeField]
    private RectTransform uiElement;            // GEM 이펙트 오브젝트가 이동할 목표 위치.
    [SerializeField]
    private UnityEvent onGemCollectEvent;       // GEM 이동이 완료되는 순간에 호출할 메소드를 등록하는 이벤트.

    private MemoryPool memoryPool;              // GEM 생성 및 활성, 비활성을 관리하는 메모리 풀.

    private void Awake()
    {
        memoryPool = new MemoryPool(gemEffectPrefab);
    }

    public void SpawnGemEffect(Vector2 point, int count = 5)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject gem = memoryPool.ActivatePoolItem(point);
            gem.GetComponent<GemCollectEffect>().Setup(this, uiElement);
        }
    }

    public void OnGemCollect(GameObject gem)
    {
        onGemCollectEvent?.Invoke();            // GEM을 획득할 때 onGemCollectEvent에 등록된 메소드 호출.
        memoryPool.DeactivatePoolItem(gem);     // 매개변수로 받아온 GEM 오브젝트 비활성화.
    }
}
