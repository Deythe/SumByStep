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
        
        if (_used && !_tileResident)
        {
            LaunchAllEffectOnLeave();
            return;
        }
        
        if (_tileResident && _tileResident.CompareTag("Player"))
        {
            LaunchAllEffectOnArrive();
            _used = true;
        }
    }

    private void LaunchAllEffectOnArrive()
    {
        foreach (var pivotTileEffect in _listTileEffects)
        {
            if (pivotTileEffect)
            {
                pivotTileEffect.ExecuteEffectOnArrive(_tileResident);
            }
        }
    }
    
    private void LaunchAllEffectOnLeave()
    {
        foreach (var pivotTileEffect in _listTileEffects)
        {
            if (pivotTileEffect)
            {
                pivotTileEffect.ExecuteEffectOnLeave(_tileResident);
            }
        }
    }
}
