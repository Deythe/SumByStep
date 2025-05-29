using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class CustomAction : MonoBehaviour, IComparable<CustomAction>
{
    public Action notifyFinished;
    public string customID {get; private set;}
        
    [SerializeField] protected UnityEvent<CustomAction> _eventForRequirement;
    protected float _currentTimeDuration;
    
    [SerializeField] private float _priorityExecuteOrder;
    private const float MAXTIMEDURATION = 0.4f;
    
    public abstract void GetDatasForAction<T>(T param);
    public abstract void ExecuteAction();
    public abstract void CancelAction();
    public abstract bool CheckCanExecuteAction();
    public abstract string SaveAction();
    public abstract void LoadAction(string datas_);
    private void Awake()
    {
        _currentTimeDuration = MAXTIMEDURATION;
        customID = IDGenerator.GenerateID(GetType().Name);
    }

    public int CompareTo(CustomAction other)
    {
        if (other == null) return 1;
        
        return _priorityExecuteOrder.CompareTo(other._priorityExecuteOrder);
    }
    public void UpdateCurrentTimeDuration(int numberAllActions)
    {
        _currentTimeDuration = MAXTIMEDURATION/numberAllActions;
    }

    public void GhostAction()
    {
        StartCoroutine(CoroutineGhostAction());
    }
    private IEnumerator CoroutineGhostAction()
    {
        yield return new WaitForSeconds(_currentTimeDuration);
        notifyFinished.Invoke();
    }
}
