using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{ 
    [SerializeField] private List<TileEffect> _listTileEffects;
    
    private Transform _tileResident;
    private bool _used;

    private void Awake()
    {
        _used = false;
    }

    public void UpdateTileResident(Transform newTileTransform)
    {
        _tileResident = newTileTransform;
        
        CheckNewResident();
    }
    
    private void CheckNewResident()
    {
        if (!_tileResident || _used) return;

        if (_tileResident.CompareTag("Player"))
        {
            LaunchAllEffect();
            _used = true;
        }
    }

    private void LaunchAllEffect()
    {
        foreach (var pivotTileEffect in _listTileEffects)
        {
            pivotTileEffect.ExecuteEffect(_tileResident);
        }
    }
}
