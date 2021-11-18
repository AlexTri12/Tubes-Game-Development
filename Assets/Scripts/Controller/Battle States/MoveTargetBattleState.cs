using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetBattleState : BattleState
{
    List<Tile> tiles;

    public override void Enter()
    {
        base.Enter();
        Movement mover = turn.actor.GetComponentInChildren<Movement>();
        tiles = mover.GetTilesInRange(board);
        board.SelectTile(tiles);
        RefreshPrimaryStatsPanel(pos);

        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerHighlightMoveTarget());
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTile(tiles);
        tiles = null;
        statsPanelController.HidePrimary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
        RefreshPrimaryStatsPanel(pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (tiles.Contains(owner.currentTile))
                owner.ChangeState<MoveSequenceState>();
        }
        else
        {
            owner.ChangeState<CommandSelectionState>();
        }
    }

    IEnumerator ComputerHighlightMoveTarget()
    {
        Point cursorPos = pos;
        while (!cursorPos.Equals(turn.plan.moveLocation))
        {
            if (cursorPos.x < turn.plan.moveLocation.x) cursorPos.x++;
            if (cursorPos.x > turn.plan.moveLocation.x) cursorPos.x--;
            if (cursorPos.y < turn.plan.moveLocation.y) cursorPos.y++;
            if (cursorPos.y > turn.plan.moveLocation.y) cursorPos.y--;

            SelectTile(cursorPos);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        owner.ChangeState<MoveSequenceState>();
    }
}
