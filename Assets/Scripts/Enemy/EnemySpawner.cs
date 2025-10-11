using System.Collections.Generic; // �Ϲ�ȭ ����Ʈ ��� List<T>.
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
    private EntityBase target;
    [SerializeField]
    private int enemyCount = 10;

    private Vector3 offset = new Vector3(0.5f, 0.5f, 0);
    private List<Vector3> possibleTiles = new List<Vector3>();

    public static List<EntityBase> Enemies {  get; private set; } = new List<EntityBase>();

    private void Awake()
    {
        // Tilemap�� Bounds �缳�� (���� �������� �� Bounds�� ������� �ʴ� ���� �ذ�).
        tilemap.CompressBounds();

        // Ÿ�ϸ��� ��� Ÿ���� ������� �� ��ġ�� ������ Ÿ�� ���.
        CalculatePossibleTiles();

        // ������ Ÿ�Ͽ� 10���� �� ����.
        for (int i = 0; i < enemyCount; ++i)
        {
            int type = Random.Range(0, enemyPrefabs.Length);
            int index = Random.Range(0, possibleTiles.Count);

            GameObject clone = Instantiate(enemyPrefabs[type], possibleTiles[index], Quaternion.identity, transform);
            clone.GetComponent<EnemyBase>().Initialize(this, parentTransform);
            clone.GetComponent<EnemyFSM>().Setup(target);

            // ������ ���� ������ ����Ʈ�� �߰�.
            Enemies.Add(clone.GetComponent<EntityBase>());
        }
    }

    private void CalculatePossibleTiles()
    {
        BoundsInt bounds = tilemap.cellBounds;
        TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

        // �ܰ��� ���� �پ��ִ� Ÿ���� �����ϱ� ����,
        // x, y�� ���� ���� 1, �� ���� bounds.size.x - 1, bounds.size.y - 1�� ����.
        for (int y = 1; y < bounds.size.y - 1; ++y)
        {
            for (int x = 1; x < bounds.size.x - 1; ++x)
            {
                TileBase tile = allTiles[y * bounds.size.x + x];

                if (tile != null)
                {
                    // ���� Ÿ���� ���� ��ǥ�� �����ͼ� 3D ���� ��ǥ�� ��ȯ.
                    Vector3Int localPosition = bounds.position + new Vector3Int(x, y);
                    // ���� ���� ��ǥ�� �̸� ������ offset�� ����.
                    Vector3 position = tilemap.CellToWorld(localPosition) + offset;
                    // z�� ��ǥ�� 0���� ����.
                    position.z = 0;

                    // ���������� ���� Ÿ���� ���� ��ǥ�� ����Ʈ�� �߰�.
                    possibleTiles.Add(position);
                }
            }
        }
    }

    public void Deactivate(EntityBase enemy)
    {
        // �Ű������� �޾ƿ� ��(enemy)�� Enemies ����Ʈ���� ������.
        Enemies.Remove(enemy);

        // ���忡 �ִ� �� ������Ʈ(enemy.gameObject)�� ������.
        Destroy(enemy.gameObject);
    }
}
