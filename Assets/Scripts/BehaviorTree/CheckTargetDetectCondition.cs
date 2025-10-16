using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance]", category: "Conditions", id: "6a84ce18ac9e5a4bd38929f7bb0f3fb2")]
public partial class CheckTargetDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    [SerializeReference] public BlackboardVariable<float> ChaseDistance;

    public override bool IsTrue()
    {
        if (CurrentDistance.Value <= ChaseDistance.Value)
        {
            return true;
        }

        return false;
    }
}
