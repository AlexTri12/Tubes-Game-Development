using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierFeature : Feature
{
    public StatsTypes type;
    public int amount;

    Stats stats
    {
        get { return _target.GetComponentInParent<Stats>(); }
    }

    protected override void OnApply()
    {
        stats[type] += amount;
    }

    protected override void OnRemove()
    {
        stats[type] -= amount;
    }
}
