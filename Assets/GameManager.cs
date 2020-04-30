using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private UIMapManager uiMapManager;

    private Vector2Int cell_select = new Vector2Int(0, 0);
    private int nbrOfPlayer = 10;

    [SerializeField] private GameObject Player;

    public BFS bfs;

    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("Map").GetComponent<MapManager>();
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        mapManager.GenerateMap();
        mapManager.Refresh();

        //while (nbrOfPlayer != 0)
        //{
        //    int i = Random.Range(0, 20), j = Random.Range(0, 20);
        //    if (mapManager.GetMap().matrix[i,j].get_type() != 1)
        //    {
        //        nbrOfPlayer--;
        //        var player = GameObject.Instantiate(Player);
        //        player.transform.position = new Vector3(-5 + 0.5f * i, -5 + 0.5f * j);
        //        mapManager.GetMap().AddObject(i, j, player);
        //    }
        //}

        bfs = new BFS();
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

    public void SetCellSelect()
    {
        List<Cell> cells = bfs.Research(this.cell_select.x, this.cell_select.y, 40, this.mapManager.GetMap());
        uiMapManager.SetTile(cells);

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
