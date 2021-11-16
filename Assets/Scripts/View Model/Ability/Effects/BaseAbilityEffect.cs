using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityEffect : MonoBehaviour
{
    protected const int minDamage = -999;
    protected const int maxDamage = 999;

    public const string GetAttackNotification = "BaseAbilityEffect.getAttackNotification";
    public const string GetDefenseNotification = "BaseAbilityEffect.getDefenseNotification";
    public const string GetPowerNotification = "BaseAbilityEffect.getPowerNotification";
    public const string TweakDamageNotification = "BaseAbilityEffect.tweakDamageNotification";

    public const string MissedNotification = "BaseAbilityEffect.missedNotification";
    public const string HitNotification = "BaseAbilityEffect.hitNotification";

    public abstract int Predict(Tile target);
    protected abstract int OnApply(Tile target);

    public void Apply(Tile target)
    {
        if (GetComponent<AbilityEffectTarget>().IsTarget(target) == false)
            return;

        if (GetComponent<HitRate>().RollForHit(target))
            this.PostNotification(HitNotification, OnApply(target));
        else
            this.PostNotification(MissedNotification);
    }

    protected virtual int GetStats(Unit attacker, Unit target, string notification, int startValue)
    {
        var mods = new List<ValueModifier>();
        var info = new Info<Unit, Unit, List<ValueModifier>>(attacker, target, mods);
        this.PostNotification(notification, info);
        mods.Sort(Compare);

        float value = startValue;
        for (int i = 0; i < mods.Count; ++i)
            value = mods[i].Modify(startValue, value);

        int retValue = Mathf.FloorToInt(value);
        retValue = Mathf.Clamp(retValue, minDamage, maxDamage);
        return retValue;
    }

    int Compare(ValueModifier x, ValueModifier y)
    {
        return x.sortOrder.CompareTo(y.sortOrder);
    }
}
