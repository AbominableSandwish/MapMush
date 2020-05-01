using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private UIMapManager uiMapManager;

    private Vector2Int cell_select = new Vector2Int(0, 0);
    private int nbrOfPlayer = 100;

    [SerializeField] private GameObject Player;

    public Search search;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        Generate();
    }

    public void Generate()
    {
        uiMapManager.Refresh();
        uiMapManager.Refresh2();
        mapManager.GenerateMap();
        mapManager.Refresh();
        while (nbrOfPlayer != 0)
        {
            int i = Random.Range(0, 19), j = Random.Range(0, 19);
            if (mapManager.GetMap().matrix[i, j].get_type() != 1)
            {
                nbrOfPlayer--;
                var player = GameObject.Instantiate(Player);
                player.GetComponent<IAController>().SetPosition(new Vector2Int(i, j));
                player.transform.position = new Vector3(-5 + 0.5f * i, -5 + 0.5f * j);
                mapManager.GetMap().AddObject(i, j, player);
            }
        }
        //StartCoroutine(Search(0.2f));
        SetCellSelect();
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
        //cells = search.Research(this.cell_select.x, this.cell_select.y, 40);
        //uiMapManager.SetTile(cells);
        //Map map = mapManager.GetMap();

        //path = search.Path(new Vector2Int(this.cell_select.x, this.cell_select.y), new Vector2Int(19,19));
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
