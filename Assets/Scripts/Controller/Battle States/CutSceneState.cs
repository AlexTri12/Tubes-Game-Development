using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneState : BattleState
{
    ConversationController conversationController;
    ConversationData data;

    protected override void Awake()
    {
        base.Awake();
        conversationController = owner.GetComponentInChildren<ConversationController>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (data)
            Resources.UnloadAsset(data);
    }

    public override void Enter()
    {
        base.Enter();
        if (IsBattleOver())
        {
            if (DidPlayerWin())
                data = Resources.Load<ConversationData>("Conversations/OutroSceneWin");
            else
                data = Resources.Load<ConversationData>("Conversations/OutroSceneLose");
        }
        else
        {
            data = Resources.Load<ConversationData>("Conversations/Tutorial");
        }
        conversationController.Show(data);
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation;
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        base.OnFire(sender, e);
        conversationController.Next();
    }

    void OnCompleteConversation(object sender, System.EventArgs eventArgs)
    {
        if (IsBattleOver())
            owner.ChangeState<EndBattleState>();
        else
            owner.ChangeState<SelectUnitState>();
    }
}
