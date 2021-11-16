using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveAbilityEffect : BaseAbilityEffect
{
    public float percent;

    protected override int OnApply(Tile target)
    {
        Stats s = target.content.GetComponent<Stats>();
        int value = s[StatsTypes.HP] = Predict(target);
        return value;
    }

    public override int Predict(Tile target)
    {
        Stats s = target.content.GetComponent<Stats>();
        return Mathf.FloorToInt(s[StatsTypes.MHP] * percent);
    }
}
