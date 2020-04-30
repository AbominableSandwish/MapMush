using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIMapManager : MonoBehaviour
{
    [SerializeField] private Tilemap uiTileMap;
    [SerializeField] private TileBase cursorCell;

    List<Cell> cellsTarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
      
    }
    //public void SetTile(Vector2Int position)
    //{
    //   // Refresh();
    //    uiTileMap.SetTile(new Vector3Int(position.x, position.y, 0),  cursorCell);
    //}

    public void SetTile(List<Cell> cells)
    {
        StopCoroutine(Show(0.002f));
        this.cellsTarget = cells;
        Refresh();
        StartCoroutine(Show(0.002f));


    }

    public void AddTile(Cell cell)
    {
        uiTileMap.SetTile(new Vector3Int((int)cell.position.x, (int)cell.position.y, 0), cursorCell);
    }

    public void Refresh()
    {
        uiTileMap.ClearAllTiles();
    }

    IEnumerator Show(float time)
    {

        int index = 0;
        while (cellsTarget.Count != 0)
        {
            Cell cell = cellsTarget[0];
            uiTileMap.SetTile(new Vector3Int((int)cell.position.x, (int)cell.position.y, 0), cursorCell);
            cellsTarget.Remove(cell);
            yield return new WaitForSeconds(time);
        }
        
    }


}
