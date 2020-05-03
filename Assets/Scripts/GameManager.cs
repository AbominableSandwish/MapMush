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
    [SerializeField] private int nbrOfPlayer = 20;
    
    [SerializeField] private GameObject Player;

    public Search search;


    [SerializeField] GameObject Player2;
    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        iaManager = GameObject.Find("IAManager").GetComponent<IAManager>();
        Generate();
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f / 2)));
        return screenCoord;
    }

    public void Generate()
    {
        uiMapManager.Refresh();
        uiMapManager.Refresh2();
        mapManager.GenerateMap();
        mapManager.Refresh();

        int counter = 0;
        while (counter < nbrOfPlayer )
        {
   
            int i, j;
            if (counter < nbrOfPlayer / 2)
            {
                i = Random.Range(0, mapManager.GetMap().GetWidth()/2);
                j = Random.Range(0, mapManager.GetMap().GetHeight());
            }
            else
            {
                i = Random.Range(0, mapManager.GetMap().GetWidth()/2) + mapManager.GetMap().GetWidth() / 2;
                j = Random.Range(0, mapManager.GetMap().GetHeight());
            }
           
            if (mapManager.GetMap().matrix[i, j].get_type() != 1)
            {
             
                GameObject player;
                if (counter < nbrOfPlayer/2){
                    player = GameObject.Instantiate(Player);
                }else
                {
                    player = GameObject.Instantiate(Player2);
                }
                player.GetComponent<IAController>().SetPosition(new Vector2Int(i, j));
                player.transform.position = convertTileCoordInScreenCoord(i, j);
                //player.transform.position = new Vector3(-5 + 0.5f * i, -5 + 0.5f * j);
                mapManager.GetMap().AddObject(i, j, player);
                iaManager.AddIA(player.GetComponent<IAController>());
                counter++;

            }
        }
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
