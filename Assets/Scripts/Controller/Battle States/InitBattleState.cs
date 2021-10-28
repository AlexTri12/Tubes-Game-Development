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
        SpawnTestUnits();
        yield return null;
        owner.ChangeState<CutSceneState>();
    }

    void SpawnTestUnits()
    {
        System.Type[] components = new System.Type[]
        {
            typeof(WalkMovement),
            typeof(FlyMovement),
            typeof(TeleportMovement)
        };

        for (int i = 0; i < components.Length; ++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();

            if (i == 0)
                owner.currentUnit = unit;

            Movement m = instance.AddComponent(components[i]) as Movement;
            m.range = 4;
            m.jumpHeight = 3;
        }
    }
}
