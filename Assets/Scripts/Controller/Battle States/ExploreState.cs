using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreState : BattleState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatsPanel(pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }

    public override void Enter()
    {
        base.Enter();
        RefreshPrimaryStatsPanel(pos);
    }

    public override void Exit()
    {
        base.Exit();
        statsPanelController.HidePrimary();
    }
}
