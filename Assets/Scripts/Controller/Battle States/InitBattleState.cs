using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        yield return null;
        owner.ChangeState<MoveTargetBattleState>();
    }
}
