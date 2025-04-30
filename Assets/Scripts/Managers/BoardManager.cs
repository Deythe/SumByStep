using System;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;
    public List<Vector2> listTiles {get; private set; }

    [SerializeField] private Transform _tileParent;

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
        if (!_tileParent.Equals(null))
        {
            FeedListTiles(_tileParent);
        } 
    }

    public bool HaveTileHere(Vector2 tilePosition)
    {
        return listTiles.Contains(tilePosition);
    }
    private void FeedListTiles(Transform parent)
    {
        listTiles = new List<Vector2>();
        
        for (int i = 0; i < _tileParent.childCount; i++)
        {
            Vector3 currentPosition = parent.GetChild(i).position;
            listTiles.Add(new Vector2(currentPosition.x, currentPosition.z));
        }
    }
}
