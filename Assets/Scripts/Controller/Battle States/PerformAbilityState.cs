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
        // Turn the character so the character faces the target
        for (int i = 0; i < 4; i++)
        {
            turn.actor.dir = (Directions)i;
            if (turn.targets[0].content.GetComponent<Unit>().GetFacing(turn.actor) == Facings.Front)
            {
                turn.actor.Match();
                break;
            }
        }

        // Attack animation and getting hit animation
        if (turn.ability == turn.actor.GetComponentInChildren<Ability>())
            // Check if the ability is normal attack or not
            turn.animation.Attack();
        else
            // If ability is special
            turn.animation.SpecialAttack();
        for (int i = 0; i < turn.targets.Count; ++i)
            turn.targets[i].content.GetComponent<UnitAnimation>().GetHit();
        
        yield return new WaitForSeconds(1.5f);
        // Reset triggers
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
