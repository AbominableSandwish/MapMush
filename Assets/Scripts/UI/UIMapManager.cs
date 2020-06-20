using System.Collections;
using System.Collections.Generic;
using MapGame;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIMapManager : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Transform transformParent;
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite cursorCell;

    List<Cell>[] paths;
    private List<Transform>[] pathsView;

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    int FindFree()
    {
        int index = 0;
        for (int i = 0; i < paths.Length - 1; i++)
        {
            if (paths[i] == null)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    public int AddPath(List<Cell> path)
    {
        if (this.paths == null)
        {
            this.paths = new List<Cell>[GameObject.Find("IAManager").GetComponent<IAManager>().maxInAction];
            this.pathsView = new List<Transform>[GameObject.Find("IAManager").GetComponent<IAManager>().maxInAction];
        }

        int index = FindFree();
        this.paths[index] = path;
        ShowPath(index);
        return index;
    }

    public void RemovePath(int index)
    {
        foreach (var CellUi in this.pathsView[index])
        {
            Destroy(CellUi.gameObject);
        }
        this.paths[index] = null;
    }

    public void ShowPath(int index)
    {
        List<Transform> view = new List<Transform>();
        foreach (Cell cell in this.paths[index])
        {
          
            Vector2 positionCell = convertTileCoordInScreenCoord((int)cell.position.x, (int)cell.position.y);
            Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(0, GetComponent<MapManager>().GetMap().matrix[(int)cell.position.x, (int)cell.position.y].position.z+0.75f);
            var cellUI = Instantiate(prefab, positionMap, Quaternion.identity, transform);
            cellUI.GetComponent<SpriteRenderer>().sortingOrder =
                ((height - (int)cell.position.y) +
                 (width - (int)cell.position.x)) * 3 + 2;
            view.Add(cellUI.transform);
        }

        pathsView[index] = view;
    }


    public void CleanPath(int index)
    {
        // uiTileMap.ClearAllTiles();
    }
}
