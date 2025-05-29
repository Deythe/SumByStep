using UnityEngine;
using UnityEngine.Serialization;

public class TE_ModifyHP : TileEffect
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private int _lifePointModifier;
    
    public override void ExecuteEffect(Transform tileResident_)
    {
        tileResident_.GetComponent<LifeBehaviour>().UpdateLifePoint(_lifePointModifier);
    }

    public override void CancelEffect(Transform tileResident_)
    {
        
    }
    
    private void OnValidate()
    {
        UpdateMeshWithDigit();
    }

    private void UpdateMeshWithDigit()
    {
        if (_meshRenderer == null ) return;
        
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        _meshRenderer?.GetPropertyBlock(block);
        block.SetFloat("_Number", _lifePointModifier);
        _meshRenderer?.SetPropertyBlock(block);
    }

    public void SetLifePointModifier(int modifier)
    {
        _lifePointModifier = modifier;
        UpdateMeshWithDigit();
    }
}
