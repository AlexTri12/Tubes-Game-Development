using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderController : MonoBehaviour
{
    const int turnActivation = 1000;
    const int turnCost = 500;
    const int moveCost = 300;
    const int actionCost = 200;

    public const string RoundBeganNotification = "TurnOrderController.roundBegan";
    public const string TurnCheckNotification = "TurnOrderController.turnCheck";
    public const string TurnCompletedNotification = "TurnOrderController.turnCompleted";
    public const string RoundEndedNotification = "TurnOrderController.roundEnded";
    public const string TurnBeganNotification = "TurnOrderController.turnBeganNotification";

    public IEnumerator Round()
    {
        BattleController bc = GetComponent<BattleController>();
        int counter = 0;
        while (counter < 200)
        {
            this.PostNotification(RoundBeganNotification);

            List<Unit> units = new List<Unit>(bc.units);
            for (int i = 0; i < units.Count; ++i)
            {
                Stats s = units[i].GetComponent<Stats>();
                s[StatsTypes.CTR] += s[StatsTypes.SPD];
            }

            units.Sort((a, b) => GetCounter(a).CompareTo(GetCounter(b)));

            for (int i = units.Count - 1; i >= 0; --i)
            {
                if (CanTakeTurn(units[i]))
                {
                    bc.turn.Change(units[i]);
                    units[i].PostNotification(TurnBeganNotification);
                    yield return units[i];

                    int cost = turnCost;
                    if (bc.turn.hasUnitMoved)
                        cost += moveCost;
                    if (bc.turn.hasUnitActed)
                        cost += actionCost;

                    Stats s = units[i].GetComponent<Stats>();
                    s.SetValue(StatsTypes.CTR, s[StatsTypes.CTR] - cost, false);

                    units[i].PostNotification(TurnCompletedNotification);
                }
            }

            this.PostNotification(RoundEndedNotification);
            counter++;
        }
    }

    bool CanTakeTurn(Unit target)
    {
        BaseException exc = new BaseException(GetCounter(target) >= turnActivation);
        target.PostNotification(TurnCheckNotification, exc);
        return exc.toogle;
    }

    int GetCounter(Unit target)
    {
        return target.GetComponent<Stats>()[StatsTypes.CTR];
    }
}
