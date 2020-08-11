using System.Collections;
using System.Collections.Generic;
using MapGame;
using UnityEditor.Experimental;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private Map map;
    private MapManager mapManager;
    public BufferGraphic bufferGraphic;
    // private UIMapManager uiMapManager;
    //private IAManager iaManager;
    private CameraManager cameraManager;

    private Vector2Int cell_select = new Vector2Int(0, 0);




    //public Search search;

    private int teamIsPlaying = 0;


    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    //public void Select(Vector2 position)
    //{
    //    mapManager.SelectCell(position, Color.cyan);
    //}



    public void  SetEndTurn()
    {
        if (teamIsPlaying == 1)
        {
            teamIsPlaying = 0;
        }
        else
        {
            teamIsPlaying = 1;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //Todo buffer
        Generate();
        //teamIsPlaying = 1;
    }




    public void Generate()
    {
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
       // uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
      //  iaManager = GameObject.Find("IAManager").GetComponent<IAManager>();
        cameraManager = Camera.main.gameObject.GetComponent<CameraManager>();

        bufferGraphic = new BufferGraphic(mapManager);

        mapManager.GenerateMap(cameraManager);
        //if(iaManager.isActiveAndEnabled)
        //    iaManager.InstancePlayer(mapManager);
        cameraManager.RestartCamera(bufferGraphic);
    }

    public void ChangeCellSelect(Vector2Int movePosition)
    {
        if ((this.cell_select.x + movePosition.x >= 0 && this.cell_select.x + movePosition.x < 20)
            && (this.cell_select.y + movePosition.y >= 0 && this.cell_select.y + movePosition.y < 20))
        {
            this.cell_select += movePosition;
            SetCellSelect();
        }
    }

    private List<Cell> cells;
    private List<Cell> path;

    public void SetCellSelect()
    {
        //search = new Search();
       // cells = search.Research(this.cell_select.x, this.cell_select.y, 40);
       // uiMapManager.SetTile(cells);

        //path = search.Path(new Vector2Int(this.cell_select.x, this.cell_select.y), new Vector2Int(19, 19));
        //uiMapManager.SetTile2(path);

    }

    //public void SetCellSelect(Vector2Int newPosition)
    //{
    //    this.cell_select = newPosition;
    //    uiMapManager.SetTile(cells);

    //    Debug.Log(this.cell_select.x + " " + this.cell_select.y);
    //}
}
