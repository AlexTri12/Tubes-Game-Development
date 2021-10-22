using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        Movement m = owner.currentUnit.GetComponent<Movement>();
        yield return StartCoroutine(m.Traverse(owner.currentTile));
        owner.ChangeState<SelectUnitState>();
    }
}
