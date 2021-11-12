using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STypeHitRate : HitRate
{
    public override int Calculate(Unit attacker, Unit target)
    {
        if (AutomaticMiss(attacker, target))
            return Final(100);

        if (AutomaticHit(attacker, target))
            return Final(0);

        int res = GetResistance(target);
        res = AdjustForStatusEffects(attacker, target, res);
        res = AdjustForRelativeFacing(attacker, target, res);
        res = Mathf.Clamp(res, 0, 100);
        return Final(res);
    }

    int GetResistance(Unit target)
    {
        Stats s = target.GetComponentInParent<Stats>();
        return s[StatsTypes.RES];
    }

    int AdjustForRelativeFacing(Unit attacker, Unit target, int rate)
    {
        switch (attacker.GetFacing(target))
        {
            case Facings.Front:
                return rate;
            case Facings.Side:
                return rate - 10;
            default: // Back
                return rate - 20;
        }
    }
}
