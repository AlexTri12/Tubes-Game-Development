using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit Recipe")]
public class UnitRecipe : ScriptableObject
{
    public string model;
    public string job;
    public string attack;
    public string abilityCatalog;
    public Locomotions locomotion;
    public Alliances alliance;
    public string strategy;
}
