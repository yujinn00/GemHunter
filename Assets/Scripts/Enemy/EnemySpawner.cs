using System.Collections;
using System.Collections.Generic; // 일반화 리스트 사용 List<T>.
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject enemySpawnTile;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GemCollector gemCollector;
    [SerializeField]
    private EntityBase target;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();
    private MemoryPool enemySpawnTilePool;                      // 타일을 리스트에 저장해두고 생성, 활성, 비활성 관리를 하는 변수.
    private WaitForSeconds waitTime = new WaitForSeconds(2f);   // 타일 등장 후 적이 생성될 때까지 걸리는 시간.
    public static UnityEvent exitEvent = new UnityEvent();      // 현재 스테이지에 존재하는 모든 적이 사망했을 때 호출할 메소드를 등록하는 변수.

    public static List<EntityBase> Enemies {  get; private set; } = new List<EntityBase>();

    // 적이 사용하는 하나의 순찰 경로를 저장할 구조체.
    [System.Serializable]
    private struct WayPointData
    {
        // 순찰 경로 오브젝트들을 저장할 배열 변수.
        public GameObject[] wayPoints;
    }
    // 순찰 경로를 여러 개 생성해두고, 적마다 임의의 경로를 선택할 수 있도록 하기 위한 위 구조체 타입의 배열 변수.
    [SerializeField]
    private WayPointData[] wayPointData;

    private void Awake()
    {
        // enemySpawnTilePool이 관리하는 오브젝트를 enemySpawnTile로 설정하고, 메모리를 할당.
        enemySpawnTilePool = new MemoryPool(enemySpawnTile);

        // Tilemap의 Bounds 재설정 (맵을 수정했을 때 Bounds가 변경되지 않는 문제 해결).
        tilemap.CompressBounds();

        // 타일맵의 모든 타일을 대상으로 적 배치가 가능한 타일 계산.
        CalculatePossibleTiles();
    }

    public void SpawnEnemies(int count)
    {
        Enemies.Clear();
        StartCoroutine(nameof(Process), count);
    }

    private IEnumerator Process(int count)
    {
        Vector3[] positions = new Vector3[count];
        for (int i = 0; i < count; ++i)
        {
            // 적을 배치할 임의의 위치 설정.
            positions[i] = possibleTiles[Random.Range(0, possibleTiles.Count)];

            // 적이 배치될 위치에 타일 생성.
            enemySpawnTilePool.ActivatePoolItem(positions[i]);
        }

        yield return waitTime;

        // 모든 타일 삭제.
        enemySpawnTilePool.DeactivateAllPoolItems();

        // 적 생성.
        for (int i = 0; i < count; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int wayIndex = Random.Range(0, wayPointData.Length);

            GameObject clone = Instantiate(enemyPrefabs[type], positions[i], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform, gemCollector);
            clone.GetComponent<EnemyFSM>().Setup(target, wayPointData[wayIndex].wayPoints);

            // 생성한 적의 정보를 리스트에 추가.
            Enemies.Add(clone.GetComponent<EntityBase>());
        }
    }

    private void CalculatePossibleTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // 외곽의 벽과 붙어있는 타일은 제외하기 위해,
        // x, y의 시작 값은 1, 끝 값은 bounds.size.x - 1, bounds.size.y - 1로 설정.
        for (int y = 1; y < bounds.size.y - 1; ++y)
        {
            for (int x = 1; x < bounds.size.x - 1; ++x)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];

                if (tile != null)
                {
                    // 현재 타일의 로컬 좌표를 가져와서 3D 월드 좌표로 변환.
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    // 계산된 월드 좌표에 미리 설정된 offset을 더함.
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    // z축 좌표를 0으로 설정.
                    position.z = 0;

                    // 최종적으로 계산된 타일의 월드 좌표를 리스트에 추가.
                    possibleTiles.Add(position);
                }
            }
        }
    }

    public void Deactivate(EntityBase enemy)
    {
        // 매개변수로 받아온 적(enemy)을 Enemies 리스트에서 삭제함.
        Enemies.Remove(enemy);

        // 월드에 있는 적 오브젝트(enemy.gameObject)를 삭제함.
        Destroy(enemy.gameObject);

        if (Enemies.Count == 0)
        {
            // 현재 필드에 존재하는 적이 없으면, exitEvent에 등록되어 있는 메소드를 호출함.
            exitEvent?.Invoke();
        }
    }
}
