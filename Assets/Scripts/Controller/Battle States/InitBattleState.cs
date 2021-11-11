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
        owner.round = owner.gameObject.AddComponent<TurnOrderController>().Round();
        yield return null;
        owner.ChangeState<CutSceneState>();
    }

    void SpawnTestUnits()
    {
        string[] jobs = new string[] { "Warrior", "Wizard" };
        for (int i = 0; i < jobs.Length; ++i)
        {
            GameObject instance = Instantiate(owner.heroPrefab) as GameObject;

            Stats s = instance.AddComponent<Stats>();
            s[StatsTypes.LVL] = 1;

            GameObject jobPrefab = Resources.Load<GameObject>("Jobs/" + jobs[i]);
            GameObject jobInstance = Instantiate(jobPrefab) as GameObject;
            jobInstance.transform.SetParent(instance.transform);

            Job job = jobInstance.GetComponent<Job>();
            job.Employ();
            job.LoadDefaultStats();

            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);

            Unit unit = instance.GetComponent<Unit>();
            unit.Place(board.GetTile(p));
            unit.Match();

            units.Add(unit);
        }
    }
}
