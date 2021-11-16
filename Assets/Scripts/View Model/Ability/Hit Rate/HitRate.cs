using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitRate : MonoBehaviour
{
    public const string AutomaticHitCheckNotification = "HitRate.automaticHitCheckNotification";
    public const string AutomaticMissCheckNotification = "HitRate.automaticMissCheckNotification";
    public const string StatusCheckNotification = "HitRate.statusCheckNotification";

    protected Unit attacker;

    protected virtual void Start()
    {
        attacker = GetComponentInParent<Unit>();
    }

    public abstract int Calculate(Tile target);

    public virtual bool RollForHit(Tile target)
    {
        int roll = UnityEngine.Random.Range(0, 101);
        int chance = Calculate(target);
        return roll <= chance;
    }

    protected virtual bool AutomaticHit(Unit target)
    {
        MatchExceptions exc = new MatchExceptions(attacker, target);
        this.PostNotification(AutomaticHitCheckNotification, exc);
        return exc.toogle;
    }

    protected virtual bool AutomaticMiss(Unit target)
    {
        MatchExceptions exc = new MatchExceptions(attacker, target);
        this.PostNotification(AutomaticMissCheckNotification, exc);
        return exc.toogle;
    }

    protected virtual int AdjustForStatusEffects(Unit target, int rate)
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
