using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoStatusController : MonoBehaviour
{
    private void OnEnable()
    {
        this.AddObserver(OnHPDidChangeNotification, Stats.DidChangeNotifications(StatsTypes.HP));
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHPDidChangeNotification, Stats.DidChangeNotifications(StatsTypes.HP));
    }

    void OnHPDidChangeNotification(object sender, object args)
    {
        Stats stats = sender as Stats;
        if (stats[StatsTypes.HP] == 0)
        {
            Status status = stats.GetComponentInChildren<Status>();
            StatsComparisonStatusCondition condition = status.Add<KnockOutStatusEffect, StatsComparisonStatusCondition>();
            condition.Init(StatsTypes.HP, 0, condition.EqualTo);
        }
    }
}
