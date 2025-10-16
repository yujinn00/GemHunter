using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wander", story: "[Self] Navigate To WanderPosition", category: "Action", id: "6d9685bbc1ec29f06d2dc5a3361dab36")]
public partial class WanderAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private NavMeshAgent agent;             // 적 목표 위치 설정을 위한 변수.
    private Vector3 wanderPosition;         // 배회 위치.
    private float currentWanderTime = 0f;   // 배회 시간 계산을 위한 현재 배회 시간.
    private float maxWanderTime = 5f;       // 배회 시간 계산을 위한 최대 배회 시간.

    protected override Status OnStart()
    {
        int jitterMin = 0;                                                  // 배회 위치 설정을 위한 최소 각도.
        int jitterMax = 360;                                                // 배회 위치 설정을 위한 최대 각도.
        float wanderRadius = UnityEngine.Random.Range(2.5f, 6f);            // 현재 위치를 원점으로 하는 원의 반지름.
        int wanderJitter = UnityEngine.Random.Range(jitterMin, jitterMax);  // 선택된 각도 (jitterMin ~ jitterMax).

        // 목표 위치 = 자신(Self)의 위치 + 각도(wanderJitter)에 해당하는 반지름(wanderRadius) 크기의 원의 둘레 위치.
        wanderPosition = Self.Value.transform.position + Utils.GetPositionFromAngle(wanderRadius, wanderJitter);
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(wanderPosition);
        currentWanderTime = Time.time;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if ((wanderPosition - Self.Value.transform.position).sqrMagnitude < 0.1f || Time.time - currentWanderTime > maxWanderTime)
        {
            return Status.Success;
        }

        return Status.Running;
    }
}
