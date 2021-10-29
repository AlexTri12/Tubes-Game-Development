using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;
    public GameObject heroPrefab;
    public Unit currentUnit;
    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();
    public Tile currentTile
    {
        get { return board.GetTile(pos); }
    }

    void Start()
    {
        ChangeState<InitBattleState>();
    }
}
