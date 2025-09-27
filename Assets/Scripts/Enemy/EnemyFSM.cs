using System.Collections;
using UnityEngine;

public enum EnemyState { None = -1, Attack, }

public class EnemyFSM : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;

    private EnemyBase owner;
    private EnemyState enemyState;

    private void Awake()
    {
        owner = GetComponent<EnemyBase>();

        ChangeState(EnemyState.Attack);
    }

    public void Setup(EntityBase target)
    {
        owner.Target = target;
    }

    public void ChangeState(EnemyState newState)
    {
        // ������ ��� ���̴� ���� ����.
        StopCoroutine(enemyState.ToString());

        // ���� ����.
        enemyState = newState;

        // ���ο� ���� ���.
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
