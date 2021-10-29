using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rank : MonoBehaviour
{
    public const int minLevel = 1;
    public const int maxLevel = 99;
    public const int maxExperience = 99999;

    Stats stats;

    public int LVL
    {
        get { return stats[StatsTypes.LVL]; }
    }
    public int EXP
    {
        get { return stats[StatsTypes.EXP]; }
        set { stats[StatsTypes.EXP] = value; }
    }
    public float LevelPercent
    {
        get { return (float)(LVL - minLevel) / (float)(maxLevel - minLevel); }
    }

    void Awake()
    {
        stats = GetComponent<Stats>();
    }

    void OnEnable()
    {
        this.AddObserver(OnExpWillChange, Stats.WillChangeNotifications(StatsTypes.EXP), stats);
        this.AddObserver(OnExpDidChange, Stats.DidChangeNotifications(StatsTypes.EXP), stats);
    }

    void OnDisable()
    {
        this.RemoveObserver(OnExpWillChange, Stats.WillChangeNotifications(StatsTypes.EXP), stats);
        this.RemoveObserver(OnExpDidChange, Stats.DidChangeNotifications(StatsTypes.EXP), stats);
    }

    void OnExpWillChange(object sender, object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, EXP, maxExperience));
    }

    void OnExpDidChange(object sender, object args)
    {
        stats.SetValue(StatsTypes.LVL, LevelForExperience(EXP), false);
    }

    public static int ExperienceForLevel(int level)
    {
        float levelPercent = Mathf.Clamp01((float)(level - minLevel) / (float)(maxLevel - minLevel));
        return (int)EasingEquations.EaseInQuad(0, maxExperience, levelPercent);
    }

    public static int LevelForExperience(int exp)
    {
        int lvl = maxLevel;
        for (; lvl >= minLevel; --lvl)
            if (exp >= ExperienceForLevel(lvl))
                break;
        return lvl;
    }

    public void Init(int level)
    {
        stats.SetValue(StatsTypes.LVL, level, false);
        stats.SetValue(StatsTypes.EXP, ExperienceForLevel(level), false);
    }
}
