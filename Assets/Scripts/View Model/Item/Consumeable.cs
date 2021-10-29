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
}
