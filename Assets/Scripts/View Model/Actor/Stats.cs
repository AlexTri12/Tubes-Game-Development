using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int this[StatsTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    int[] _data = new int[(int)StatsTypes.Count];

    // Notifications
    static Dictionary<StatsTypes, string> _willChangeNotifications = new Dictionary<StatsTypes, string>();
    static Dictionary<StatsTypes, string> _didChangeNotifications = new Dictionary<StatsTypes, string>();
    
    public static string WillChangeNotifications(StatsTypes type)
    {
        if (!_willChangeNotifications.ContainsKey(type))
            _willChangeNotifications.Add(type, string.Format("Stats.{0} will change", type.ToString()));
        return _willChangeNotifications[type];
    }

    public static string DidChangeNotifications(StatsTypes type)
    {
        if (!_didChangeNotifications.ContainsKey(type))
            _didChangeNotifications.Add(type, string.Format("Stats.{0} did change", type.ToString()));
        return _didChangeNotifications[type];
    }

    public void SetValue(StatsTypes type, int value, bool allowExceptions)
    {
        int oldValue = this[type];
        if (oldValue == value)
            return;

        if (allowExceptions)
        {
            // Allow exceptions to the rule here
            ValueChangeException exc = new ValueChangeException(oldValue, value);

            // The notification is unique per stat type
            this.PostNotification(WillChangeNotifications(type), exc);

            // Did anything modify the value?
            value = Mathf.FloorToInt(exc.GetModifiedValue());

            // Did something nullify the change?
            if (exc.toogle == false || value == oldValue)
                return;
        }

        _data[(int)type] = value;
        this.PostNotification(DidChangeNotifications(type), oldValue);
    }
}
