using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AddStatusFeature<T> : Feature where T : StatusEffect
{
    StatusCondition statusCondition;

    protected override void OnApply()
    {
        Status status = GetComponentInParent<Status>();
        statusCondition = status.Add<T, StatusCondition>();
    }

    protected override void OnRemove()
    {
        if (statusCondition != null)
            statusCondition.Remove();
    }
}
