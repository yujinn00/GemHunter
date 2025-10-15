using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    // 메모리 풀로 관리되는 오브젝트 정보.
    private class PoolItem
    {
        public GameObject gameObject;               // 화면에 보이는 실제 게임 오브젝트.
        private bool isActive;                      // "gameObject"의 활성화 및 비활성화 정보.

        public bool IsActive
        {
            set
            {
                isActive = value;
                gameObject.SetActive(isActive);
            }
            get => isActive;
        }
    }

    private GameObject poolObject;                  // 오브젝트 풀링에서 관리하는 게임 오브젝트 프리팹.
    private List<PoolItem> poolItemList;            // 관리되는 모든 오브젝트를 저장하는 리스트.

    private readonly int increaseCount = 5;         // 오브젝트가 부족할 때 추가로 생성할 오브젝트 개수.

    public int MaxCount { private set; get; }       // 현재 리스트에 등록되어 있는 오브젝트 개수.
    public int ActiveCount { private set; get; }    // 현재 게임에 사용되고 있는 활성 상태의 오브젝트 개수.

    // 오브젝트가 임시로 보관되는 위치.
    private Vector3 tempPosition = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

    // 메모리를 할당할 때 호출되는 생성자.
    public MemoryPool(GameObject poolObject)
    {
        MaxCount = 0;
        ActiveCount = 0;
        this.poolObject = poolObject;

        poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    // 오브젝트를 increaseCount 개수만큼 생성하는 메소드.
    public void InstantiateObjects()
    {
        MaxCount += increaseCount;

        for (int i = 0; i < increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.gameObject = GameObject.Instantiate(poolObject);
            poolItem.gameObject.transform.position = tempPosition;
            poolItem.IsActive = false;

            poolItemList.Add(poolItem);
        }
    }

    // 현재 관리 중인 모든 오브젝트를 삭제하는 메소드.
    public void DestroyObjects()
    {
        if (poolItemList == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject.Destroy(poolItemList[i].gameObject);
        }

        poolItemList.Clear();
    }

    // poolItemList에 사용 중이 아닌(비활성화) 오브젝트 중 하나를 선택해 사용하는(활성화) 메소드.
    // 현재 모든 오브젝트가 사용 중이면 InstantiateObjects() 메소드로 추가 생성.
    public GameObject ActivatePoolItem(Vector3 position)
    {
        if (poolItemList == null)
        {
            return null;
        }

        // 현재 생성해서 관리하는 모든 오브젝트 개수와 현재 활성화 상태인 오브젝트 비교.
        // 모든 오브젝트가 활성화 상태이면 새로운 오브젝트 필요.
        if (MaxCount == ActiveCount)
        {
            InstantiateObjects();
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.IsActive == false)
            {
                ActiveCount++;

                poolItem.gameObject.transform.position = position;
                poolItem.IsActive = true;

                return poolItem.gameObject;
            }
        }

        return null;
    }

    // 사용이 끝난 오브젝트를 비활성화하는 메소드.
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (poolItemList == null || removeObject == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject == removeObject)
            {
                ActiveCount--;

                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;

                return;
            }
        }
    }

    // 게임에 사용 중인 모든 오브젝트를 비활성화하는 메소드.
    public void DeactivateAllPoolItems()
    {
        if (poolItemList == null)
        {
            return;
        }

        int count = poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = poolItemList[i];

            if (poolItem.gameObject != null && poolItem.IsActive == true)
            {
                poolItem.IsActive = false;
                poolItem.gameObject.transform.position = tempPosition;
            }
        }

        ActiveCount = 0;
    }
}
