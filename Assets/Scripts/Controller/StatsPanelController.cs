using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsPanelController : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";

    [SerializeField] StatsPanel primaryPanel;
    [SerializeField] StatsPanel secondaryPanel;

    Tweener primaryTransition;
    Tweener secondaryTransition;

    void Start()
    {
        if (primaryPanel.panel.CurrentPosition == null)
            primaryPanel.panel.SetPosition(HideKey, false);
        if (secondaryPanel.panel.CurrentPosition == null)
            secondaryPanel.panel.SetPosition(HideKey, false);
    }

    public void ShowPrimary(GameObject obj)
    {
        primaryPanel.Display(obj);
        MovePanel(primaryPanel, ShowKey, ref primaryTransition);
    }

    public void HidePrimary()
    {
        MovePanel(primaryPanel, HideKey, ref primaryTransition);
    }

    public void ShowSecondary(GameObject obj)
    {
        secondaryPanel.Display(obj);
        MovePanel(secondaryPanel, ShowKey, ref secondaryTransition);
    }

    public void HideSecondary()
    {
        MovePanel(secondaryPanel, HideKey, ref secondaryTransition);
    }

    void MovePanel(StatsPanel obj, string pos, ref Tweener t)
    {
        Position target = obj.panel[pos];
        if (obj.panel.CurrentPosition != target)
        {
            if (t != null)
                t.Stop();
            t = obj.panel.SetPosition(pos, true);
            t.duration = 0.5f;
            t.equation = EasingEquations.EaseOutQuad;
        }
    }
}
