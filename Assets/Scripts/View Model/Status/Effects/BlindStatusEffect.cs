using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindStatusEffect : StatusEffect
{
    private void OnEnable()
    {
        this.AddObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHitRateStatusCheck, HitRate.StatusCheckNotification);
    }

    void OnHitRateStatusCheck(object sender, object args)
    {
        Info<Unit, Unit, int> info = args as Info<Unit, Unit, int>;
        Unit owner = GetComponentInParent<Unit>();
        if (owner == info.arg0)
        {
            // The attacker is blind, add the evasion rate
            info.arg2 += 50;
        }
        else if (owner == info.arg1)
        {
            // The defender is blind, reduce the evasion rate
            info.arg2 -= 20;
        }
    }
}
