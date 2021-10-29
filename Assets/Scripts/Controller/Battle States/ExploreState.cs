using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreState : BattleState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        /* 
        GameObject content = owner.currentTile.content;

        if (content != null)
        {
            owner.currentUnit = content.GetComponent<Unit>();
            owner.ChangeState<MoveTargetBattleState>();
        }
        */

        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}
