using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ActionAndPriorities
{
    public ActionsEnum currentAction;
    public int priorityExecuteOrder;
}

public class ActionsBehaviour : MonoBehaviour
{
    [SerializeField] private List<ActionAndPriorities> actions;

    private void Start()
    {
        ActionsManager currentInstance = ActionsManager.Instance;
        if (!currentInstance.Equals(null))
        {
            currentInstance.RegisterActionBehaviour(this);
        }
    }

    public void ExecuteMyActions()
    {
        actions.Sort();
        foreach (var action in actions)
        {
            //action.currentAction.DoAction(ref parameter);
        }
    }

    public void UndoLastAction()
    {
        actions.Sort();
        foreach (var action in actions)
        {
            //action.currentAction.CancelAction();
        }
    }
}
