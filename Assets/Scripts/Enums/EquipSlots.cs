using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EquipSlots
{
    None = 0,
    Primary = 1 << 0,       // Usually a weapon
    Secondary = 1 << 1,     // Usually shield, but could be sword (dual-wield) or two-handed weapon
    Head = 1 << 2,          // Helmet, hat, etc
    Body = 1 << 3,          // Body armor, robe, etc
    Accessory = 1 << 4      // Ring, belt, etc
}
