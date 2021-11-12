using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalAbilityPower : BaseAbilityPower
{
    public int level;

    protected override int GetBaseAttack()
    {
        return GetComponentInParent<Stats>()[StatsTypes.ATK];
    }

    protected override int GetBaseDefense(Unit target)
    {
        return target.GetComponent<Stats>()[StatsTypes.DEF];
    }

    protected override int GetPower()
    {
        return level;
    }
}
