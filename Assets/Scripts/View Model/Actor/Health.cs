using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int HP
    {
        get { return stats[StatsTypes.HP]; }
        set { stats[StatsTypes.HP] = value; }
    }
    public int MHP
    {
        get { return stats[StatsTypes.MHP]; }
        set { stats[StatsTypes.MHP] = value; }
    }
    public int MinHP = 0;
    Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnHPWillChange, Stats.WillChangeNotifications(StatsTypes.HP), stats);
        this.AddObserver(OnMHPDidChange, Stats.DidChangeNotifications(StatsTypes.MHP), stats);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHPWillChange, Stats.WillChangeNotifications(StatsTypes.HP), stats);
        this.RemoveObserver(OnMHPDidChange, Stats.DidChangeNotifications(StatsTypes.MHP), stats);
    }

    void OnHPWillChange(object sender, object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, MinHP, stats[StatsTypes.MHP]));
    }

    void OnMHPDidChange(object sender, object args)
    {
        int oldMHP = (int)args;
        if (MHP > oldMHP)
            HP += MHP - oldMHP;
        else
            HP = Mathf.Clamp(HP, MinHP, MHP);
    }
}
