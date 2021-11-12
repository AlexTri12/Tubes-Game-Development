using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasteStatusEffect : StatusEffect
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
        MultDeltaModifier m = new MultDeltaModifier(0, 2);
        exc.AddModifier(m);
    }
}
