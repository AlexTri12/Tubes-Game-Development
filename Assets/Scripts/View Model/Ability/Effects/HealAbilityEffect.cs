using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAbilityEffect : BaseAbilityEffect
{
    protected override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted value
        int value = Predict(target);

        // Add some random vairance
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        // Clamp the amount to a range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        // Apply the amount to the target
        Stats s = defender.GetComponent<Stats>();
        s[StatsTypes.HP] += value;
        return value;
    }

    public override int Predict(Tile target)
    {

        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();
        return GetStats(attacker, defender, GetPowerNotification, 0);
    }
}
