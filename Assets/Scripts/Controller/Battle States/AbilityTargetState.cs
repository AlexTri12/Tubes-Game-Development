using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityRange ar;

    public override void Enter()
    {
        base.Enter();
        ar = turn.ability.GetComponent<AbilityRange>();
        SelectTiles();
        statsPanelController.ShowPrimary(turn.actor.gameObject);
        if (ar.directionOriented)
            RefreshPrimaryStatsPanel(pos);
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTile(tiles);
        statsPanelController.HidePrimary();
        statsPanelController.HideSecondary();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (ar.directionOriented)
        {
            ChangeDirection(e.info);
        }
        else
        {
            SelectTile(e.info + pos);
            RefreshSecondaryStatsPanel(pos);
        }
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (ar.directionOriented || tiles.Contains(board.GetTile(pos)))
                owner.ChangeState<ConfirmAbilityTargetState>();
        }
        else
        {
            owner.ChangeState<CategorySelectionState>();
        }
    }

    void ChangeDirection(Point p)
    {
        Directions dir = p.GetDirection();
        if (turn.actor.dir != dir)
        {
            board.DeSelectTile(tiles);
            turn.actor.dir = dir;
            turn.actor.Match();
            SelectTiles();
        }
    }

    void SelectTiles()
    {
        tiles = ar.GetTilesInRange(board);
        board.SelectTile(tiles);
    }
}
