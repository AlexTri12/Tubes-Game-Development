using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StatsComparisonStatusCondition : StatusCondition
{
    public StatsTypes type
    {
        get;
        private set;
    }
    public int value
    {
        get;
        private set;
    }
    public Func<bool> condition
    {
        get;
        private set;
    }
    Stats stats;

    private void Awake()
    {
        stats = GetComponentInParent<Stats>();
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnStatsChanged, Stats.DidChangeNotifications(type), stats);
    }

    public void Init(StatsTypes type, int value, Func<bool> func)
    {
        this.type = type;
        this.value = value;
        this.condition = condition;
        this.AddObserver(OnStatsChanged, Stats.DidChangeNotifications(type), stats);
    }

    public bool EqualTo()
    {
        return stats[type] == value;
    }

    public bool LessThan()
    {
        return stats[type] < value;
    }

    public bool LessThanOrEqualTo()
    {
        return stats[type] <= value;
    }

    public bool GreaterThan()
    {
        return stats[type] > value;
    }

    public bool GreaterThanOrEqualTo()
    {
        return stats[type] >= value;
    }

    void OnStatsChanged(object sender, object args)
    {
        if (condition != null && !condition())
            Remove();
    }
}
