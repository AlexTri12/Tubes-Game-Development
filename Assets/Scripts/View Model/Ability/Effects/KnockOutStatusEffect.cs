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
        // Add dead animation
        owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);
        this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.AddObserver(OnStatsCounterWillChange, Stats.WillChangeNotifications(StatsTypes.CTR), stats);
    }

    private void OnDisable()
    {
        // Add revive animation
        owner.transform.localScale = Vector3.one;
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
