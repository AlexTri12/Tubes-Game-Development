using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopStatusEffect : StatusEffect
{
    Stats myStats;

    private void OnEnable()
    {
        myStats = GetComponentInParent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), myStats);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), myStats);
    }

    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        exc.FlipToogle();
    }
}
