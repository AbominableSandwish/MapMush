using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapGame;
using UnityEngine;

public class Search
{
    private UIMapManager uiMapManager;

    public class ParamSearch
    {
        public int x;
        public int y;
        public Cell parent;
        public int field;

        public ParamSearch(int x, int y, Cell parent, int field)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
            this.field = field;
        }
    }

    private List<Cell> fieldOfView;
    // Get Path
    private List<Cell> path;
    private List<ParamSearch> queue;
    private GameObject unit_select;
    private Map map;

    public List<Cell> Research(int x, int y, int field)
    {
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        map = GameObject.Find("Map").GetComponent<MapManager>().GetMap();

        //if (map.matrix[x, y].get_type() == 1)
        //{
        //    return null;
        //}

        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int j = 0; j < map.GetHeight(); j++)
            {
                map.matrix[i,j].came_from = null;
                map.matrix[i,j].visited = false;
            }
        }

        this.fieldOfView = new List<Cell>();
        this.queue = new List<ParamSearch>();


        if (map.matrix[x, y].Object != null)
            unit_select = map.matrix[x, y].Object;
        queue.Add(new ParamSearch(x, y, map.matrix[x, y], field));

        while (queue.Count() > 0)
        {
            var next_tile = queue[0];

            BFS_search(next_tile.x, next_tile.y, next_tile.parent, next_tile.field);

            queue.Remove(queue[0]);
        }     
        return this.fieldOfView;
    }

    public void BFS_search(int x, int y, Cell parent, int field)
    {
        if ((x >= 0 && x <= map.GetWidth() - 1) && (y >= 0 && y <= map.GetHeight() - 1)){
            if (map.matrix[x,y].get_type() >= 1)
            {
                if (map.matrix[x, y].visited == false)
                {
                    map.matrix[x,y].visited = true;
                    map.matrix[x,y].came_from = parent;
                    this.fieldOfView.Add(map.matrix[x,y]);
                    if (field != 0)
                    {
                        queue.Add(new ParamSearch(x + 1, y, map.matrix[x,y], field - 1));
                        queue.Add(new ParamSearch(x - 1, y, map.matrix[x, y], field - 1));
                            
                        queue.Add(new ParamSearch(x, y + 1, map.matrix[x, y], field - 1));
                        queue.Add(new ParamSearch(x, y - 1, map.matrix[x, y], field - 1));
                    }
                }
            }
        }
    }

    public List<Cell> Path(Vector2Int positionStart, Vector2Int positionTarget)
    {

        if (map.matrix[positionTarget.x, positionTarget.y].get_type() == 0)
        {
            return null;
        }

        this.path = new List<Cell>();
        Cell cellStart = map.matrix[positionStart.x, positionStart.y];
        Cell cellTarget = map.matrix[positionTarget.x, positionTarget.y];
        foreach (var cell in fieldOfView)
        {
            if (cell.position == cellTarget.position)
            {
                path.Add(cell);
            }
        }
        
        int i = 0;
        while (path[path.Count - 1] != cellStart)
        {
            

            path.Add(path[path.Count - 1].came_from);

            if (i >= 100)
            {
                path = null;
                return path;
            }
            i++;
        }
        path.Reverse();

        return path;
    }
}

