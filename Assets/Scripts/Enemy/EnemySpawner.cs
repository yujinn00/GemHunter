using System.Collections.Generic; // 일반화 리스트 사용 List<T>.
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private Transform parentTransform;
    [SerializeField]
    private GemCollector gemCollector;
    [SerializeField]
    private EntityBase target;
    [SerializeField]
    private int enemyCount = 10;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

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
        // Tilemap의 Bounds 재설정 (맵을 수정했을 때 Bounds가 변경되지 않는 문제 해결).
        tilemap.CompressBounds();

        // 타일맵의 모든 타일을 대상으로 적 배치가 가능한 타일 계산.
        CalculatePossibleTiles();

        // 임의의 타일에 10명의 적 생성.
        for (int i = 0; i < enemyCount; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int index = Random.Range(0, possibleTiles.Count);
            int wayIndex = Random.Range(0, wayPointData.Length);

            GameObject clone = Instantiate(enemyPrefabs[type], possibleTiles[index], Quaternion.identity, transform);
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
    }
}
