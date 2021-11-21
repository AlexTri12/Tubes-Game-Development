using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformItemState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        turn.hasUnitActed = true;
        if (turn.hasUnitMoved)
            turn.lockMove = true;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        // Turn the character so the character faces the target
        Directions startDir = turn.actor.dir;
        for (int i = 0; i < 4; i++)
        {
            turn.actor.dir = (Directions)i;
            if (turn.targets[0].content.GetComponent<Unit>().GetFacing(turn.actor) == Facings.Front)
            {
                turn.actor.Match();
                break;
            }
        }
        
        turn.animation.SpecialAttack();
        yield return new WaitForSeconds(1.5f);
        // Reset triggers
        turn.animation.ResetTriggers();
        turn.actor.dir = startDir;
        turn.actor.Match();

        ApplyItem();

        if (IsBattleOver())
            owner.ChangeState<CutSceneState>();
        else if (!UnitHasControl())
            owner.ChangeState<SelectUnitState>();
        if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();
    }

    void ApplyItem()
    {
        for (int i = 0; i < turn.targets.Count; ++i)
            turn.consumeable.Consume(turn.targets[i]);
        
        Consumeable c = turn.consumeable;
        turn.consumeable = null;
        Destroy(c.gameObject);
    }

    bool UnitHasControl()
    {
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }
}
