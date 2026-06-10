using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class TE_FinishLevel : TileEffect
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _finishMaterial;
   
    public override void ExecuteEffectOnLeave(Transform tileResident_)
    {
    }

    public override void ExecuteEffectOnArrive(Transform tileResident_)
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void CancelEffect(Transform tileResident_) {}
    
    private void OnValidate()
    {
        if (_meshRenderer && _finishMaterial)
        {
            _meshRenderer.material = _finishMaterial;
        }
    }
}
