using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] Navigate To [Target]", category: "Action", id: "0383743ee847d7f7e6d6287c0c4b02d2")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private NavMeshAgent agent;     // 적 이동 제어를 위한 변수.
    private EntityBase target;      // 적의 목표 위치 확인을 위한 변수.

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        target = Target.Value.GetComponent<EntityBase>();

        agent.speed = 5f;
        agent.SetDestination(target.MiddlePoint);

        return Status.Running;
    }
}
