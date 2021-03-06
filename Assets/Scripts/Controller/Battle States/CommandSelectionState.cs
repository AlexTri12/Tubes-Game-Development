using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectionState : BaseAbilityMenuState
{
    protected override void Cancel()
    {
        if (turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.tile.pos);
        }
        else
        {
            owner.ChangeState<ExploreState>();
        }
    }

    protected override void Confirm()
    {
        switch (abilityMenuPanelController.selection)
        {
            case 0:
                owner.ChangeState<MoveTargetBattleState>();
                break;
            case 1:
                owner.ChangeState<CategorySelectionState>();
                break;
            case 2:
                owner.ChangeState<ItemSelectionState>();
                break;
            case 3:
                owner.ChangeState<EndFacingState>();
                break;
        }
    }

    protected override void LoadMenu()
    {
        if (menuOptions == null)
        {
            menuTitle = "Commands";
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Action");
            menuOptions.Add("Item");
            menuOptions.Add("Wait");
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, turn.hasUnitMoved);
        abilityMenuPanelController.SetLocked(1, turn.hasUnitActed);
        abilityMenuPanelController.SetLocked(2, turn.hasUnitActed);
    }

    public override void Enter()
    {
        base.Enter();
        statsPanelController.ShowPrimary(turn.actor.gameObject);
        turn.animation.IdleState();

        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerTurn());
    }

    public override void Exit()
    {
        base.Exit();
        statsPanelController.HidePrimary();
    }

    IEnumerator ComputerTurn()
    {
        if (turn.plan == null)
        {
            turn.plan = owner.cpu.Evaluate();
            turn.ability = turn.plan.ability;
        }

        yield return new WaitForSeconds(1f);

        if (turn.hasUnitMoved == false && !turn.plan.moveLocation.Equals(turn.actor.tile.pos))
            owner.ChangeState<MoveTargetBattleState>();
        else if (turn.hasUnitActed == false && turn.plan.ability != null)
            owner.ChangeState<AbilityTargetState>();
        else
            owner.ChangeState<EndFacingState>();
    }
}
