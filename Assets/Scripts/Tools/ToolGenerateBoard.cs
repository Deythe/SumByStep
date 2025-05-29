using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
struct PairMaterials
{
    public Material materialImpair;
    public Material materialPair;
}

public class ToolGenerateBoard : MonoBehaviour
{
    [SerializeField] private GameObject _tileType;
    [SerializeField] private List<PairMaterials> _listPairMaterials;
    
    [Header("Grid Settings")] public int width = 5;
    public int height = 5;

    [Header("HP Settings")] public int initialHP = 10;
    public int minHPChange = -3;
    public int maxHPChange = 3;

    [Header("Path Settings")] public int numberOfPaths = 1;
    public int pathLength = 8;

    [ContextMenu("Generate Level")]
    public void GenerateLevel()
    {
        int[,] grid = new int[width, height];

        for (int p = 0; p < numberOfPaths; p++)
        {
            bool pathFound = false;

            while (!pathFound)
            {
                Vector2Int start = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
                Vector2Int current = start;
                HashSet<Vector2Int> visited = new HashSet<Vector2Int> { current };
                List<Vector2Int> path = new List<Vector2Int> { current };

                int currentHP = initialHP;
                grid[start.x, start.y] = 0; // Start cell

                for (int step = 0; step < pathLength; step++)
                {
                    List<Vector2Int> neighbors = GetUnvisitedNeighbors(current, visited);
                    if (neighbors.Count == 0) break;

                    Vector2Int next = neighbors[Random.Range(0, neighbors.Count)];
                    visited.Add(next);
                    current = next;
                    path.Add(current);

                    int hpChange = Random.Range(minHPChange, maxHPChange + 1);

                    // Ensure HP never drops below zero
                    if (currentHP + hpChange <= 0)
                    {
                        hpChange = Mathf.Max(1, maxHPChange);
                    }

                    currentHP += hpChange;
                    grid[current.x, current.y] = hpChange;
                }

                if (path.Count >= pathLength)
                {
                    grid[current.x, current.y] = 999; // End marker
                    pathFound = true;
                    Debug.Log($"Path {p + 1}: " + string.Join(" -> ", path));
                }
            }
        }
        
        GenerateBoard(ref grid);
    }

    private List<Vector2Int> GetUnvisitedNeighbors(Vector2Int pos, HashSet<Vector2Int> visited)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        List<Vector2Int> neighbors = new List<Vector2Int>();

        foreach (var dir in directions)
        {
            Vector2Int neighbor = pos + dir;
            if (neighbor.x >= 0 && neighbor.x < width && neighbor.y >= 0 && neighbor.y < height && !visited.Contains(neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
    
    private void GenerateBoard(ref int[,] newGrid)
    {
        Transform board = new GameObject("Parent").transform;

        board.name = "Board";
        board.AddComponent<TilesManager>();
        
        int randomIndexPair = Random.Range(0, _listPairMaterials.Count); 
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject tile = null;
                tile = Instantiate(_tileType, new Vector3(x, 0, y), Quaternion.identity, board);
                TileBehaviour component = tile.GetComponent<TileBehaviour>();
                
                if ((y + x) % 2 == 0)
                {
                    
                    component.GetComponent<MeshRenderer>().material = _listPairMaterials[randomIndexPair].materialImpair;
                }
                else
                {
                    component.GetComponent<MeshRenderer>().material = _listPairMaterials[randomIndexPair].materialPair;
                }

                tile.name = "Tile" + x + "," + y;
                component?.GetComponent<TE_ModifyHP>()?.SetLifePointModifier(newGrid[x,y]);
            }
        }
    }
}
