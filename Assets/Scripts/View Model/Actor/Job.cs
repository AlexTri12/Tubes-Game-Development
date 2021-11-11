using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : MonoBehaviour
{
    public static readonly StatsTypes[] statsOrder = new StatsTypes[]
    {
        StatsTypes.MHP,
        StatsTypes.MMP,
        StatsTypes.ATK,
        StatsTypes.DEF,
        StatsTypes.MAT,
        StatsTypes.MDF,
        StatsTypes.DEF,
    };

    public int[] baseStats = new int[statsOrder.Length];
    public float[] growStats = new float[statsOrder.Length];
    Stats stats;

    private void OnDestroy()
    {
        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotifications(StatsTypes.LVL));
    }

    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
        this.AddObserver(OnLvlChangeNotification, Stats.DidChangeNotifications(StatsTypes.LVL), stats);

        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Active(gameObject);
    }

    public void UnEmploy()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for (int i = 0; i < features.Length; ++i)
            features[i].Deactive();

        this.RemoveObserver(OnLvlChangeNotification, Stats.DidChangeNotifications(StatsTypes.LVL), stats);
        stats = null;
    }

    public void LoadDefaultStats()
    {
        for (int i = 0; i < statsOrder.Length; ++i)
        {
            StatsTypes type = statsOrder[i];
            stats.SetValue(type, baseStats[i], false);
        }

        stats.SetValue(StatsTypes.HP, stats[StatsTypes.MHP], false);
        stats.SetValue(StatsTypes.MP, stats[StatsTypes.MMP], false);
    }

    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StatsTypes.LVL];

        for (int i = oldValue; i < newValue; ++i)
            LevelUp();
    }

    void LevelUp()
    {
        for (int i = 0; i < statsOrder.Length; ++i)
        {
            StatsTypes type = statsOrder[i];
            int whole = Mathf.FloorToInt(growStats[i]);
            float fraction = growStats[i] - whole;

            int value = stats[type];
            value += whole;
            if (UnityEngine.Random.value > (1f - fraction))
                value++;

            stats.SetValue(type, value, false);
        }

        stats.SetValue(StatsTypes.HP, stats[StatsTypes.MHP], false);
        stats.SetValue(StatsTypes.MP, stats[StatsTypes.MMP], false);
    }
}
