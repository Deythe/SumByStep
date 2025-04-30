using Unity.VisualScripting;
using UnityEngine;

public class ToolGenerate : MonoBehaviour
{
   [SerializeField] private Vector2 _size;
   [SerializeField] private GameObject _tileType1;
   [SerializeField] private GameObject _tileType2;

   [ContextMenu("Generate Simple Board")]
   public void GenerateSimpleBoard()
   {
      Transform board = new GameObject("Parent").transform;
      
      board.name = "Board";
      board.AddComponent<BoardManager>();
      
      for (int y = 0; y < _size.y; y++)
      {
         for (int x = 0; x < _size.x; x++)
         {
            GameObject tile = null;
            
            if ((y + x) % 2 == 0) {
               tile = Instantiate(_tileType1, new Vector3(x,0,y), Quaternion.identity, board);
            }
            else {
               tile = Instantiate(_tileType2, new Vector3(x,0,y), Quaternion.identity, board);
            }
            
            tile.name = "Tile" + x + "," + y;
         }
      }
   }
}
