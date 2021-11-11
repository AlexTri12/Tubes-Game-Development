using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }

    IEnumerator ChangeCurrentUnit()
    {
        owner.round.MoveNext();
        SelectTile(turn.actor.tile.pos);
        RefreshPrimaryStatsPanel(pos);
        yield return null;
        owner.ChangeState<CommandSelectionState>();
    }

    public override void Exit()
    {
        base.Exit();
        statsPanelController.HidePrimary();
    }
}
