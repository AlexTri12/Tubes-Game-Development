using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    public int MP
    {
        get { return stats[StatsTypes.MP]; }
        set { stats[StatsTypes.MP] = value; }
    }
    public int MMP
    {
        get { return stats[StatsTypes.MMP]; }
        set { stats[StatsTypes.MMP] = value; }
    }
    Unit unit;
    Stats stats;

    private void Awake()
    {
        stats = GetComponent<Stats>();
        unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnMPWillChange, Stats.WillChangeNotifications(StatsTypes.MP), stats);
        this.AddObserver(OnMMPDidChange, Stats.DidChangeNotifications(StatsTypes.MMP), stats);
        this.AddObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnMPWillChange, Stats.WillChangeNotifications(StatsTypes.MP), stats);
        this.RemoveObserver(OnMMPDidChange, Stats.DidChangeNotifications(StatsTypes.MMP), stats);
        this.RemoveObserver(OnTurnBegan, TurnOrderController.TurnBeganNotification, unit);
    }

    void OnMPWillChange(object sender, object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StatsTypes.MMP]));
    }

    void OnMMPDidChange(object sender, object args)
    {
        int oldMMP = (int)args;
        if (MMP > oldMMP)
            MP += MMP - oldMMP;
        else
            MP = Mathf.Clamp(MP, 0, MMP);
    }

    void OnTurnBegan(object sender, object args)
    {
        if (MP < MMP)
            MP += Mathf.Max(Mathf.FloorToInt(MMP * 0.1f), 1);
    }
}
