using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1, Attack, }

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

    private EnemyBase owner;
    private NavMeshAgent navMeshAgent;          // 적 이동 경로 설정 및 이동 제어.
    private EnemyState enemyState;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        ChangeState(EnemyState.Attack);
    }

    public void Setup(EntityBase target)
    {
        owner.Target = target;
        // [Debug Test] 게임이 시작된 직후 플레이어 위치로 이동.
        navMeshAgent.SetDestination(target.MiddlePoint);
    }

    public void ChangeState(EnemyState newState)
    {
        // 이전에 재생 중이던 상태 종료.
        StopCoroutine(enemyState.ToString());

        // 상태 변경.
        enemyState = newState;

        // 새로운 상태 재생.
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Attack()
    {
        var wait = new WaitForSeconds(owner.Stats.GetStat(StatType.CooldownTime).Value);

        while (true)
        {
            yield return wait;

            Vector3 target = owner.Target.MiddlePoint;
            GameObject clone = Instantiate(projectilePrefab);
            clone.transform.position = projectileSpawnPoint.position;
            clone.GetComponent<EnemyProjectile>().Setup(target, owner.Stats.GetStat(StatType.Damage).Value);
        }
    }
}
