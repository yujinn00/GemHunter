using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireDragon : ProjectileGlobal
{
    // 현재 스테이지의 정보를 가지고 있는 변수.
    [SerializeField]
    private StageData stageData;
    // 용 이미지 비활성 처리를 위한 변수.
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    // 용의 이동 범위는 카메라의 Min/Max 범위를 사용하기 때문에,
    // 용이 맵 아래쪽 바깥에서 위쪽 바깥으로 사라지도록 +-하는 변수.
    private float weight = 14f; // (이미지 크기에 따라 수정).
    // 초당 이동 거리.
    private float distancePerSecond = 20;
    // 이동 시간.
    private float moveTime;
    // Lerp() 메소드에 사용할 변수.
    private float t = 0f;
    // 용 이미지의 등장/종료 위치.
    private Vector3 start, end;
    // 현재 월드에 존재하는 적 리스트 변수.
    private List<EntityBase> entities;

    public override void Setup(SkillBase skillBase, float damage)
    {
        base.Setup(skillBase, damage);

        // 화룡 이미지의 등장/종료 위치 설정.
        start = new Vector3(0, stageData.CameraLimitMin.y - weight, 0);
        end = new Vector3(0, stageData.CameraLimitMax.y + weight, 0);
        transform.position = start;

        // 이동 시간 계산 (맵 크기가 다르더라도 동일한 속도로 이동).
        moveTime = Vector3.Distance(start, end) / distancePerSecond;

        // 현재 월드에 존재하는 모든 적 목록을 entites에 저장.
        entities = new List<EntityBase>(EnemySpawner.Enemies);

        // entites를 y 위치 값이 낮은 순으로 정렬.
        entities.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
    }

    public override void Process()
    {
        // t가 1 미만일 때 (용의 이미지를 start부터 end까지 moveTie 시간동안 이동시킴).
        if (t < 1)
        {
            t += Time.deltaTime / moveTime;
            transform.localPosition = Vector3.Lerp(start, end, t);
        }
        // t가 1 이상일 때 (용이 end 위치에 도착했을 때).
        else
        {
            // 필드에 있는 적을 공격하고 있지만 이미지가 목표에 도달했기 때문에 이미지만 비활성화.
            spriteRenderer.enabled = false;

            // 공격한 적 정보는 entities에서 빼기 때문에 entities.Count가 0이 되면 모든 적을 공격했다는 뜻임.
            // 즉, 필드에 있는 모든 적을 공격했으면 FireDragon 제거.
            if (entities.Count == 0)
            {
                Destroy(gameObject);
            }
        }

        // 공격 가능한 적이 없으면 아래 코드를 처리하지 않음.
        if (entities.Count == 0)
        {
            return;
        }

        // 현재 entities가 y 위치 기준으로 정렬되어 있기 때문에,
        // entities의 첫 번째 요소에 대해서만 FireDragon의 위치와 비교하여 처리.

        // 다른 공격에 의해 적이 사망할 수도 있기 때문에 0번 적이 null이면 entities 리스트에서 삭제.
        if (entities[0] == null)
        {
            entities.RemoveAt(0);
        }
        // 0번 적이 null이 아니고, 용이 0번 적의 y 위치보다 크거나 같으면 0번 적을 공격하고, entities 리스트에서 삭제.
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
