using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAbilityEffect : BaseAbilityEffect
{
    protected override int OnApply(Tile target)
    {
        Unit defender = target.content.GetComponent<Unit>();

        // Start with the predicted damage value
        int value = Predict(target);

        // Add some random variance
        value = Mathf.FloorToInt(value * UnityEngine.Random.Range(0.9f, 1.1f));

        // Clamp the damage to a range
        value = Mathf.Clamp(value, minDamage, maxDamage);

        // Apply the damage to the target
        Stats s = defender.GetComponent<Stats>();
        s[StatsTypes.HP] -= value;
        return value;
    }

    public override int Predict(Tile target)
    {
        Unit attacker = GetComponentInParent<Unit>();
        Unit defender = target.content.GetComponent<Unit>();

        // Get the attackers base attack stats considering mission items, support check, status check, and equipments
        int attack = GetStats(attacker, defender, GetAttackNotification, 0);

        // Get the targets base defense considering mission items, support check, status check, and equipments
        int defense = GetStats(attacker, defender, GetDefenseNotification, 0);

        // Calculate base damage
        int damage = attack - (defense / 2);
        damage = Mathf.Max(damage, 1);

        // Get the abilities power stats considering possible variations
        int power = GetStats(attacker, defender, GetPowerNotification, 0);

        // Apply power bonus
        damage = power * damage / 100;
        damage = Mathf.Max(damage, 1);

        // Tweak the damage based on a variety of other checks like elemental damage, critical hits, damage multipliers, and many others
        damage = GetStats(attacker, defender, TweakDamageNotification, damage);

        // Clamp the damage to a range
        damage = Mathf.Clamp(damage, minDamage, maxDamage);
        return damage;
    }
}
