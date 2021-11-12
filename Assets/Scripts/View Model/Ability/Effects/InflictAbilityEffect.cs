using System.Collections;
using UnityEngine;
using System.Reflection;
using System;

public class InflictAbilityEffect : BaseAbilityEffect
{
    public string statusName;
    public int duration;

    public override void Apply(Tile target)
    {
        Type statusType = Type.GetType(statusName);
        if (statusType == null || !statusType.IsSubclassOf(typeof(StatusEffect)))
        {
            Debug.LogError("Invalid status type");
            return;
        }

        MethodInfo mi = typeof(Status).GetMethod("Add");
        Type[] types = new Type[] { statusType, typeof(DurationStatusCondition) };
        MethodInfo constructed = mi.MakeGenericMethod(types);

        Status status = target.content.GetComponent<Status>();
        object retValue = constructed.Invoke(status, null);

        DurationStatusCondition condition = retValue as DurationStatusCondition;
        condition.duration = duration;
    }

    public override int Predict(Tile target)
    {
        return 0;
    }
}