using System.Collections.Generic;
using UnityEngine;

public class TilesManager : MonoBehaviour
{
    public static TilesManager Instance;

    [SerializeField] private Transform _tileParent;
    
    private List<TileBehaviour> _listTiles;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        
        Instance = this;
    }
    
    private void Start()
    {
        if (_tileParent != null)
        {
            FeedListTiles(_tileParent);
        } 
    }
    
    public bool HaveTileHere(Vector2 tilePosition)
    {
        if (_listTiles == null) return false;
        
        foreach (TileBehaviour tile in _listTiles)
        {
            if (tilePosition.Equals(new Vector2(tile.transform.position.x, tile.transform.position.z)))
            {
                return true;
            }
        }
        
        return false;
    }

    public void UpdateTile(Vector2 tilePosition, Transform onTileTransform)
    {
        foreach (var tile in _listTiles)
        {
            if(tilePosition.Equals(new Vector2(tile.transform.position.x, tile.transform.position.z)))
            {
                tile.UpdateTileResident(onTileTransform);
                return;
            }
        }
    }
    
    private void FeedListTiles(Transform parent)
    {
        _listTiles = new List<TileBehaviour>();
        
        for (int i = 0; i < _tileParent.childCount; i++)
        {
            _listTiles.Add(parent.GetChild(i).GetComponent<TileBehaviour>());
        }
    }
}
