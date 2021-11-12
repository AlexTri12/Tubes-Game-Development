using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalAbilityPower : BaseAbilityPower
{
    public int level;

    protected override int GetBaseAttack()
    {

        return GetComponentInParent<Stats>()[StatsTypes.MAT];
    }

    protected override int GetBaseDefense(Unit target)
    {

        return target.GetComponent<Stats>()[StatsTypes.MDF];
    }

    protected override int GetPower()
    {
        return level;
    }
}
