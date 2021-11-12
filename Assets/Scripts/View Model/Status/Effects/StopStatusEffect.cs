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
        this.AddObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), myStats);
        this.RemoveObserver(OnAutomaticHitCheck, HitRate.AutomaticHitCheckNotification);
    }

    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = args as ValueChangeException;
        exc.FlipToogle();
    }

    void OnAutomaticHitCheck(object sender, object args)
    {
        Unit owner = GetComponentInParent<Unit>();
        MatchExceptions exc = args as MatchExceptions;
        if (owner == exc.target)
            exc.FlipToogle();
    }
}
