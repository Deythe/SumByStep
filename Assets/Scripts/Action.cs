using System;
using UnityEngine;

public abstract class Action : IComparable<Action>
{
    private float priorityExecuteOrder;
    public abstract void DoAction<T>(ref T parameter);
    public abstract void CancelAction();
    public abstract void Save();

    public int CompareTo(Action other)
    {
        if (other == null) return 1;
        
        return priorityExecuteOrder.CompareTo(other.priorityExecuteOrder);
    }
}
