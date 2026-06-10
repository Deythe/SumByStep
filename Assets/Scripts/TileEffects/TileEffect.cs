using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    public abstract void ExecuteEffectOnLeave(Transform tileResident_);
    public abstract void ExecuteEffectOnArrive(Transform tileResident_);
    public abstract void CancelEffect(Transform tileResident_);
}