using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private UIMapManager uiMapManager;
    private IAManager iaManager;
    private CameraManager cameraManager;

    private Vector2Int cell_select = new Vector2Int(0, 0);
    


    public Search search;

    private int teamIsPlaying = 0;




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

        Generate();
        teamIsPlaying = 1;
    }

    public void Generate()
    {
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        iaManager = GameObject.Find("IAManager").GetComponent<IAManager>();
        cameraManager = Camera.main.gameObject.GetComponent<CameraManager>();
        // mapManager.Clean();
        mapManager.GenerateMap();
        iaManager.InstancePlayer(mapManager);
        cameraManager.RestartCamera();
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
        search = new Search();
        cells = search.Research(this.cell_select.x, this.cell_select.y, 40);
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
