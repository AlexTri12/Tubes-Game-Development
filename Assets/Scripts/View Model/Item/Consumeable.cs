using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumeable : MonoBehaviour
{
    public void Consume(GameObject target)
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Apply(target);
    }

    public void Consume(Tile target)
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
        {
            AbilityEffectTarget targeter = features[i].GetComponent<AbilityEffectTarget>();
            if (targeter.IsTarget(target))
                features[i].Apply(target.content.gameObject);
        }
    }

    public bool IsTarget(Tile tile)
    {
        Transform obj = transform;
        for (int i = 0; i < obj.childCount; ++i)
        {
            AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            if (targeter.IsTarget(tile))
                return true;
        }
        return false;
    }
}
