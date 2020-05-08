using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private UIMapManager uiMapManager;
    private IAManager iaManager;

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
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        iaManager = GameObject.Find("IAManager").GetComponent<IAManager>();
        Generate();
        teamIsPlaying = 1;
    }



    public void Generate()
    {
        uiMapManager.Refresh();
        uiMapManager.Refresh2();
        mapManager.GenerateMap();
        mapManager.Refresh();

        iaManager.InstancePlayer(mapManager);

        Camera.main.gameObject.GetComponent<CameraManager>().RestartCamera();
        //StartCoroutine(Search(0.2f));
        //SetCellSelect();
    }

    // Update is called once per frame
    void Update()
    {

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
        uiMapManager.SetTile(cells);

        //path = search.Path(new Vector2Int(this.cell_select.x, this.cell_select.y), new Vector2Int(19, 19));
        //uiMapManager.SetTile2(path);

    }

    //public void SetCellSelect(Vector2Int newPosition)
    //{
    //    this.cell_select = newPosition;
    //    uiMapManager.SetTile(cells);

    //    Debug.Log(this.cell_select.x + " " + this.cell_select.y);
    //}

    //IEnumerator Search(float waitTime)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(waitTime);
    //        SetCellSelect(new Vector2Int(Random.Range(0,20), Random.Range(0,20)));
    //        print("WaitAndPrint " + Time.time);
    //    }
    //}
}
