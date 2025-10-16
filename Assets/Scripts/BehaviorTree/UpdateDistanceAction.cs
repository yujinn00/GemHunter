using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateDistance", story: "Update [Self] and [Target] [CurrentDistance]", category: "Action", id: "47d3ead9b91f650601229a014ba02bae")]
public partial class UpdateDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;

    private EntityBase self;    // 적 본인의 위치 확인을 위한 변수.
    private EntityBase target;  // 적의 목표 위치 확인을 위한 변수.

    protected override Status OnStart()
    {
        self = Self.Value.GetComponent<EntityBase>();
        target = Target.Value.GetComponent<EntityBase>();

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        CurrentDistance.Value = Vector2.Distance(self.MiddlePoint, target.MiddlePoint);

        return Status.Success;
    }
}
