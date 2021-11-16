using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMagicCost : MonoBehaviour
{
    public int amount;
    Ability owner;

    private void Awake()
    {
        owner = GetComponent<Ability>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerformNotification, owner);
    }

    void OnCanPerformCheck(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatsTypes.MP] < amount)
        {
            BaseException exc = (BaseException)args;
            exc.FlipToogle();
        }
    }

    void OnDidPerformNotification(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        s[StatsTypes.MP] -= amount;
    }
}
