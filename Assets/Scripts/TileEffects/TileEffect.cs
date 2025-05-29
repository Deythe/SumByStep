using UnityEngine;

public abstract class TileEffect : MonoBehaviour
{
    public abstract void ExecuteEffect(Transform tileResident_);
    public abstract void CancelEffect(Transform tileResident_);
}