using System;
using System.Collections.Generic;
using UnityEngine;

public struct SavedAction
{
    public string actionID;
    public string actionData;

    public SavedAction(string actionID_, string actionData_)
    {
        actionID = actionID_;
        actionData = actionData_;
    }
}

public struct TickActions
{
    public int tick;
    public List<SavedAction> savedActions;

    public TickActions(int newTick_)
    {
        tick = newTick_;
        savedActions = new List<SavedAction>();
    }
}

public class ActionsBehaviour : MonoBehaviour
{
    public event Action notifyAllActionsAvailable;

    [SerializeField] private List<CustomAction> _listEquipedActions;

    private List<TickActions> _listPastActions;
    private TickActions _currentTickActions, _currentCanceledTickActions;
    private int _currentIndexAction;
    
    private void Start()
    {
        ActionsManager.Instance?.RegisterActionBehaviour(this);
        _listPastActions = new List<TickActions>();
        
        foreach (var action in _listEquipedActions)
        {
            action.notifyFinished += ExecuteCurrentActions;
        }
    }
    
    public void StartActionsSequence(int currentTick)
    {
        _currentTickActions = new TickActions(currentTick);
        _listPastActions.Add(_currentTickActions);
        
        _currentIndexAction = 0;
        _listEquipedActions?.Sort();
        
        ExecuteCurrentActions();
    }
    
    private void ExecuteCurrentActions()
    {
        if (_currentIndexAction < _listEquipedActions.Count)
        {
            _listEquipedActions[_currentIndexAction]?.UpdateCurrentTimeDuration(_listEquipedActions.Count);
            
            if (_listEquipedActions[_currentIndexAction].CheckCanExecuteAction())
            {
                _listEquipedActions[_currentIndexAction]?.ExecuteAction();
                _currentTickActions.savedActions.Add(new SavedAction(_listEquipedActions[_currentIndexAction].customID, _listEquipedActions[_currentIndexAction]?.SaveAction()));
            }else
            {
                _listEquipedActions[_currentIndexAction]?.GhostAction();
            }
            
            _currentIndexAction++;
            return;
        }
        
        notifyAllActionsAvailable?.Invoke();
    }
    
    public void StartCancelSequence()
    {
        if (_listPastActions.Count > 0)
        {
            _currentIndexAction = 0;
            _currentCanceledTickActions = _listPastActions[^1];
            _currentCanceledTickActions.savedActions.Reverse();
            _listPastActions.RemoveAt(_listPastActions.Count - 1);
            CancelLastAction();
            return;
        }

        notifyAllActionsAvailable?.Invoke();
    }
    
    private void CancelLastAction()
    {
        if (_currentIndexAction < _currentCanceledTickActions.savedActions.Count)
        {
            CustomAction pivotAction = FindActionByID(_currentCanceledTickActions.savedActions[_currentIndexAction].actionID);
            pivotAction.LoadAction(_currentCanceledTickActions.savedActions[_currentIndexAction].actionData);
            pivotAction.CancelAction();
            _currentIndexAction++;
            return;
        }
        
        notifyAllActionsAvailable?.Invoke();
    }
    
    public bool IsFirstActionsRealizable()
    {
        if (_listEquipedActions.Count<=0)
        {
            return false;
        }
        
        return _listEquipedActions[0].CheckCanExecuteAction();
    }

    private CustomAction FindActionByID(string actionID_)
    {
        foreach (var pivotActions in _listEquipedActions)
        {
            if (pivotActions.customID.Equals(actionID_))
            {
                return pivotActions;
            }
        }

        return null;
    }
}
