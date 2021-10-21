using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetBattleState : BattleState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }
}
