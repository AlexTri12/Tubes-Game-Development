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
        turn.animation.Attack();
        for (int i = 0; i < turn.targets.Count; ++i)
            turn.targets[i].content.GetComponent<UnitAnimation>().GetHit();
        yield return new WaitForSeconds(1.5f);
        turn.animation.ResetTriggers();
        for (int i = 0; i < turn.targets.Count; ++i)
            turn.targets[i].content.GetComponent<UnitAnimation>().ResetTriggers();

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
