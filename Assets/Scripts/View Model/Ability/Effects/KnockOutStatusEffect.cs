using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockOutStatusEffect : StatusEffect
{
    Unit owner;
    Stats stats;

    private void Awake()
    {
        owner = GetComponentInParent<Unit>();
        stats = owner.GetComponent<Stats>();
    }

    private void OnEnable()
    {
        owner.GetComponent<UnitAnimation>().IdleState();
        owner.GetComponent<UnitAnimation>().Die();
        this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.AddObserver(OnStatsCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), stats);
    }

    private void OnDisable()
    {
        owner.GetComponent<UnitAnimation>().Revive();
        this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.RemoveObserver(OnStatsCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), stats);
    }

    void OnTurnCheck(object sender, object args)
    {
        // Don't allow a KO'd unit to take turns
        BaseException exc = args as BaseException;
        if (exc.defaultToogle == true)
            exc.FlipToogle();
    }

    void OnStatsCounterWillChange(object sender, object args)
    {
        // Don't allow a KO'd unit to increment the turn order counter
        ValueChangeException exc = args as ValueChangeException;
        if (exc.toValue > exc.fromValue)
            exc.FlipToogle();
    }
}
