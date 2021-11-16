using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadAbilityEffectTarget : AbilityEffectTarget
{
    public bool toogle;

    public override bool IsTarget(Tile tile)
    {
        if (tile == null || tile.content == null)
            return false;

        bool hasComponent = tile.content.GetComponent<Undead>() != null;
        if (hasComponent != toogle)
            return false;

        Stats s = tile.content.GetComponent<Stats>();
        return s != null && s[StatsTypes.HP] > 0;
    }
}
