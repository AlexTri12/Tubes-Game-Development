using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatusEffect : StatusEffect
{
    Unit owner;

    private void OnEnable()
    {
        owner = GetComponentInParent<Unit>();
        if (owner)
            this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
    }

    void OnNewTurn(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        int currentHP = s[StatsTypes.HP];
        int maxHP = s[StatsTypes.MHP];
        int reduce = Mathf.Min(currentHP, Mathf.FloorToInt(maxHP * 0.1f));
        s.SetValue(StatsTypes.HP, (currentHP - reduce), false);
    }
}
