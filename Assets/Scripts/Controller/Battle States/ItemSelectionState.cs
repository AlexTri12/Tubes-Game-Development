using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSelectionState : BaseAbilityMenuState
{
    List<string> options;

    protected override void Cancel()
    {
        owner.ChangeState<CommandSelectionState>();
    }

    protected override void Confirm()
    {
        Consumeable consumeable = InventoryData.INSTANCE.Consume(options[abilityMenuPanelController.selection]);
        turn.consumeable = consumeable;
        consumeable.transform.SetParent(turn.actor.transform);
        owner.ChangeState<ItemTargetState>();
    }

    protected override void LoadMenu()
    {
        if (menuOptions == null)
            menuOptions = new List<string>();
        else
            menuOptions.Clear();

        if (options == null)
            options= new List<string>();
        else
            options.Clear();

        menuTitle = "Item";

        foreach (KeyValuePair<string, int> pair in InventoryData.INSTANCE.consumeables)
        {
            menuOptions.Add(string.Format("{0}~{1}", pair.Key, pair.Value));
            options.Add(pair.Key);
        }

        abilityMenuPanelController.Show(menuTitle, menuOptions);
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
