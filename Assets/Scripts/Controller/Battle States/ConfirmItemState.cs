using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmItemState : BattleState
{
    List<Tile> tiles;
    AbilityArea aa;
    int index = 0;

    public override void Enter()
    {
        base.Enter();
        aa = turn.consumeable.GetComponent<AbilityArea>();
        tiles = aa.GetTilesInArea(board, pos);
        board.SelectTile(tiles, new Color(0.5f, 0, 0, 0.75f));
        FindTargets();
        RefreshPrimaryStatsPanel(turn.actor.tile.pos);

        if (turn.targets.Count > 0)
        {
            // Only show this UI for Human controlled units
            if (driver.Current == Drivers.Human)
                hitSuccessIndicator.Show();
            SetTarget(0);
        }

        // Only show this UI for AI controlled units
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerDisplayAbilitySelection());
    }

    public override void Exit()
    {
        base.Exit();
        board.DeSelectTile(tiles);
        statsPanelController.HidePrimary();
        statsPanelController.HideSecondary();
        hitSuccessIndicator.Hide();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.info.y > 0 || e.info.x > 0)
            SetTarget(index + 1);
        else
            SetTarget(index - 1);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
        {
            if (turn.targets.Count > 0)
            {
                owner.bmc.Display(turn.consumeable.name);
                owner.ChangeState<PerformItemState>();
            }
        }
        else
            owner.ChangeState<ItemTargetState>();
    }

    void FindTargets()
    {
        turn.targets = new List<Tile>();
        for (int i = 0; i < tiles.Count; ++i)
            if (turn.consumeable.IsTarget(tiles[i]))
                turn.targets.Add(tiles[i]);
    }

    void SetTarget(int target)
    {
        index = target;
        if (index < 0)
            index = turn.targets.Count - 1;
        if (index >= turn.targets.Count)
            index = 0;
        if (turn.targets.Count > 0)
        {
            RefreshSecondaryStatsPanel(turn.targets[index].pos);
            UpdateHitSuccessIndicator();
        }
    }

    void UpdateHitSuccessIndicator()
    {
        int chance = 0;
        int amount = 0;
        Tile target = turn.targets[index];

        Transform obj = turn.consumeable.transform;
        for (int i = 0; i < obj.childCount; ++i)
        {
            AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            if (targeter.IsTarget(target))
            {
                HitRate hitRate = targeter.GetComponent<HitRate>();
                chance = hitRate.Calculate(target);

                StatModifierFeature feature = targeter.GetComponent<StatModifierFeature>();
                amount = feature.amount;
                break;
            }
        }

        hitSuccessIndicator.SetStats(chance, amount);
    }

    IEnumerator ComputerDisplayAbilitySelection()
    {
        owner.bmc.Display(turn.ability.name);
        yield return new WaitForSeconds(0.5f);
        owner.ChangeState<PerformItemState>();
    }
}
