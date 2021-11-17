using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAbilityState : BattleState
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
        // TODO play animations
        yield return null;
        ApplyAbility();

        if (IsBattleOver())
            owner.ChangeState<CutSceneState>();
        else if (!UnitHasControl())
            owner.ChangeState<SelectUnitState>();
        if (turn.hasUnitMoved)
            owner.ChangeState<EndFacingState>();
        else
            owner.ChangeState<CommandSelectionState>();
    }

    void ApplyAbility()
    {
        turn.ability.Perform(turn.targets);
    }

    bool UnitHasControl()
    {
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }
}
