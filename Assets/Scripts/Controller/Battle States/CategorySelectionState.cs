using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelectionState : BaseAbilityMenuState
{
    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    protected override void Confirm()
    {
        if (abilityMenuPanelController.selection == 0)
            Attack();
        else
            SetCategory(abilityMenuPanelController.selection - 1);
    }

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>();
        else
            menuOptions.Clear();

        menuTitle = "Action";
        menuOptions.Add("Attack");

        AbilityCatalog catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        for (int i = 0; i < catalog.CategoryCount(); ++i)
            menuOptions.Add(catalog.GetCategory(i).name);

        abilityMenuPanelController.Show(menuTitle, menuOptions);
    }

    void Attack()
    {
        turn.ability = turn.actor.GetComponentInChildren<Ability>();
        owner.ChangeState<AbilityTargetState>();
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        owner.ChangeState<ActionSelectionState>();
    }

    public override void Enter()
    {
        base.Enter();
        statsPanelController.ShowPrimary(turn.actor.gameObject);
    }

    public override void Exit()
    {
        base.Exit();
        statsPanelController.HidePrimary();
    }
}
