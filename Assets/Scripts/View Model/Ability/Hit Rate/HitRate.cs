using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitRate : MonoBehaviour
{
    public const string AutomaticHitCheckNotification = "HitRate.automaticHitCheckNotification";
    public const string AutomaticMissCheckNotification = "HitRate.automaticMissCheckNotification";
    public const string StatusCheckNotification = "HitRate.statusCheckNotification";

    public abstract int Calculate(Unit attacker, Unit target);

    protected virtual bool AutomaticHit(Unit attacker, Unit target)
    {
        MatchExceptions exc = new MatchExceptions(attacker, target);
        this.PostNotification(AutomaticHitCheckNotification, exc);
        return exc.toogle;
    }

    protected virtual bool AutomaticMiss(Unit attacker, Unit target)
    {
        MatchExceptions exc = new MatchExceptions(attacker, target);
        this.PostNotification(AutomaticMissCheckNotification, exc);
        return exc.toogle;
    }

    protected virtual int AdjustForStatusEffects(Unit attacker, Unit target, int rate)
    {
        Info<Unit, Unit, int> args = new Info<Unit, Unit, int>(attacker, target, rate);
        this.PostNotification(StatusCheckNotification, args);
        return args.arg2;
    }

    protected virtual int Final(int evade)
    {
        return 100 - evade;
    }
}
