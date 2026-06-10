using UnityEngine;

public class TE_Drop : TileEffect
{
    public override void ExecuteEffectOnLeave(Transform tileResident_)
    {
        Debug.Log("DROP");
    }
    public override void ExecuteEffectOnArrive(Transform tileResident_)
    {
    }

    public override void CancelEffect(Transform tileResident_)
    {
    }
}
