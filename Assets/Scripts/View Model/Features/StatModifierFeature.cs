using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifierFeature : Feature
{
    public StatsTypes type;
    public int amount;

    Stats stats
    {
        get
        {
            if (_target.GetComponentInParent<Stats>() != null)
                return _target.GetComponentInParent<Stats>();
            else
                return _target.GetComponent<Stats>();
        }
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
