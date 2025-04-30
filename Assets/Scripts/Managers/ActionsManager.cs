using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class ActionsManager : MonoBehaviour
{
    public static ActionsManager Instance;

    public Event Test ;
    
    private List<ActionsBehaviour> _actionsBehaviours;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        
        Instance = this;
    }

    public void RegisterActionBehaviour(ActionsBehaviour actionBehaviour)
    {
        _actionsBehaviours.Add(actionBehaviour);
    }

    public void ExecuteAllActions()
    {
        foreach (ActionsBehaviour actionBehaviour in _actionsBehaviours)
        {
            actionBehaviour.ExecuteMyActions();
        }
    }
    
}
