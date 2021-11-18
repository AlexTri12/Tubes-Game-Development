using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : State
{
    protected BattleController owner;
    protected Driver driver;
    public CameraRig cameraRig
    {
        get { return owner.cameraRig; }
    }
    public Board board
    {
        get { return owner.board; }
    }
    public LevelData levelData
    {
        get { return owner.levelData; }
    }
    public Transform tileSelectionIndicator
    {
        get { return owner.tileSelectionIndicator; }
    }
    public Point pos
    {
        get { return owner.pos; }
        set { owner.pos = value; }
    }
    public AbilityMenuPanelController abilityMenuPanelController
    {
        get { return owner.abilityMenuPanelController; }
    }
    public Turn turn
    {
        get { return owner.turn; }
    }
    public List<Unit> units
    {
        get { return owner.units; }
    }
    public StatsPanelController statsPanelController
    {
        get { return owner.statsPanelController; }
    }
    public HitSuccessIndicator hitSuccessIndicator
    {
        get { return owner.hitSuccessIndicator; }
    }
    protected virtual void Awake()
    {
        owner = GetComponent<BattleController>();
    }

    protected override void AddListeners()
    {
        if (driver == null || driver.Current == Drivers.Human || IsBattleOver())
        {
            InputController.moveEvent += OnMove;
            InputController.fireEvent += OnFire;
        }
    }

    protected override void RemoveListeners()
    {
        InputController.moveEvent -= OnMove;
        InputController.fireEvent -= OnFire;
    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e)
    {
        
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {
        
    }

    protected virtual void SelectTile(Point p)
    {
        if (pos == p || !board.tiles.ContainsKey(p))
            return;

        pos = p;
        tileSelectionIndicator.localPosition = board.tiles[p].center;
    }

    protected virtual Unit GetUnit(Point p)
    {
        Tile t = board.GetTile(p);
        GameObject content = t != null ? t.content : null;
        return content != null ? content.GetComponent<Unit>() : null;
    }

    protected virtual void RefreshPrimaryStatsPanel(Point p)
    {
        Unit target = GetUnit(p);
        if (target != null)
            statsPanelController.ShowPrimary(target.gameObject);
        else
            statsPanelController.HidePrimary();
    }

    protected virtual void RefreshSecondaryStatsPanel(Point p)
    {
        Unit target = GetUnit(p);
        if (target != null)
            statsPanelController.ShowSecondary(target.gameObject);
        else
            statsPanelController.HideSecondary();
    }

    protected virtual bool DidPlayerWin()
    {
        return owner.GetComponent<BaseVictoryCondition>().Victor == Alliances.Hero;
    }

    protected virtual bool IsBattleOver()
    {
        return owner.GetComponent<BaseVictoryCondition>().Victor != Alliances.None;
    }

    public override void Enter()
    {
        driver = (turn.actor != null) ? turn.actor.GetComponent<Driver>() : null;
        base.Enter();
    }
}
