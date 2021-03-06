using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour
{
    public const string CanPerformCheck = "Ability.canPerformCheck";
    public const string FailedNotification = "Ability.failedNotification";
    public const string DidPerformNotification = "Ability.didPerformNotification";

    public bool CanPerform()
    {
        BaseException exc = new BaseException(true);
        this.PostNotification(CanPerformCheck, exc);
        return exc.toogle;
    }

    public void Perform(List<Tile> targets)
    {
        if (!CanPerform())
        {
            this.PostNotification(FailedNotification);
            return;
        }

        for (int i = 0; i < targets.Count; ++i)
            Perform(targets[i]);

        this.PostNotification(DidPerformNotification);
    }

    void Perform(Tile target)
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            BaseAbilityEffect effect = child.GetComponent<BaseAbilityEffect>();
            effect.Apply(target);
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
