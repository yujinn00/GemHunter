using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireDragon : ProjectileGlobal
{
    // ���� ���������� ������ ������ �ִ� ����.
    [SerializeField]
    private StageData stageData;
    // �� �̹��� ��Ȱ�� ó���� ���� ����.
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // ���� �̵� ������ ī�޶��� Min/Max ������ ����ϱ� ������,
    // ���� �� �Ʒ��� �ٱ����� ���� �ٱ����� ��������� +-�ϴ� ����.
    private float weight = 14f; // (�̹��� ũ�⿡ ���� ����).
    // �ʴ� �̵� �Ÿ�.
    private float distancePerSecond = 20;
    // �̵� �ð�.
    private float moveTime;
    // Lerp() �޼ҵ忡 ����� ����.
    private float t = 0f;
    // �� �̹����� ����/���� ��ġ.
    private Vector3 start, end;
    // ���� ���忡 �����ϴ� �� ����Ʈ ����.
    private List<EntityBase> entities;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);

        // ȭ�� �̹����� ����/���� ��ġ ����.
        start = new Vector3(0, stageData.CameraLimitMin.y - weight, 0);
        end = new Vector3(0, stageData.CameraLimitMax.y + weight, 0);
        transform.position = start;

        // �̵� �ð� ��� (�� ũ�Ⱑ �ٸ����� ������ �ӵ��� �̵�).
        moveTime = Vector3.Distance(start, end) / distancePerSecond;

        // ���� ���忡 �����ϴ� ��� �� ����� entites�� ����.
        entities = new List<EntityBase>(EnemySpawner.Enemies);

        // entites�� y ��ġ ���� ���� ������ ����.
        entities.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
    }

    public override void Process()
    {
        // t�� 1 �̸��� �� (���� �̹����� start���� end���� moveTie �ð����� �̵���Ŵ).
        if (t < 1)
        {
            t += Time.deltaTime / moveTime;
            transform.localPosition = Vector3.Lerp(start, end, t);
        }
        // t�� 1 �̻��� �� (���� end ��ġ�� �������� ��).
        else
        {
            // �ʵ忡 �ִ� ���� �����ϰ� ������ �̹����� ��ǥ�� �����߱� ������ �̹����� ��Ȱ��ȭ.
            spriteRenderer.enabled = false;

            // ������ �� ������ entities���� ���� ������ entities.Count�� 0�� �Ǹ� ��� ���� �����ߴٴ� ����.
            // ��, �ʵ忡 �ִ� ��� ���� ���������� FireDragon ����.
            if (entities.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        // ���� ������ ���� ������ �Ʒ� �ڵ带 ó������ ����.
        if (entities.Count == 0)
        {
            return;
        }

        // ���� entities�� y ��ġ �������� ���ĵǾ� �ֱ� ������,
        // entities�� ù ��° ��ҿ� ���ؼ��� FireDragon�� ��ġ�� ���Ͽ� ó��.

        // �ٸ� ���ݿ� ���� ���� ����� ���� �ֱ� ������ 0�� ���� null�̸� entities ����Ʈ���� ����.
        if (entities[0] == null)
        {
            entities.RemoveAt(0);
        }
        // 0�� ���� null�� �ƴϰ�, ���� 0�� ���� y ��ġ���� ũ�ų� ������ 0�� ���� �����ϰ�, entities ����Ʈ���� ����.
        else
        {
            if (entities[0].transform.position.y <= transform.position.y)
            {
                TakeDamage(entities[0]);
                entities.RemoveAt(0);
            }
        }
    }
}
