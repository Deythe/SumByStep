using System.Collections.Generic;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;
    
    private int _currentTick;
    private List<ActionsBehaviour> _actionsBehaviours;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        
        Instance = this;
        
        _currentTick = 0;
        _actionsBehaviours = new List<ActionsBehaviour>();
    }

    public void RegisterActionBehaviour(ActionsBehaviour actionBehaviour)
    {
        _actionsBehaviours.Add(actionBehaviour);
    }

    public void ExecuteAllActions()
    {
        foreach (ActionsBehaviour actionBehaviour in _actionsBehaviours)
        {
            actionBehaviour.StartActionsSequence(_currentTick);
        }
        
        ++_currentTick;
    }

    public bool CancelAllActions()
    {
        if(_currentTick <= 0) return false;
        
        --_currentTick;
        
        foreach (ActionsBehaviour actionBehaviour in _actionsBehaviours)
        {
            actionBehaviour.StartCancelSequence();
        }

        return true;
    }
}
