using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BFS : MonoBehaviour
{
    private UIMapManager uiMapManager;

    class Search
    {
        public int x;
        public int y;
        public Map map;
        public int field;

        public Search(int x, int y, Map map, int field)
        {
            this.x = x;
            this.y = y;
            this.map = map;
            this.field = field;
        }
    }

    private List<Cell> path;
    private List<Search> queue;
    private GameObject unit_select;

    public List<Cell> Research(int x, int y, int field, Map map)
    {
        uiMapManager = GameObject.Find("Map").GetComponent<UIMapManager>();
        uiMapManager.Refresh();

        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int j = 0; j < map.GetHeight(); j++)
            {
                map.matrix[i,j].came_from = null;
                map.matrix[i,j].visited = false;
            }
        }

        this.path = new List<Cell>();
        this.queue = new List<Search>();

        if (map.matrix[x, y].Object != null)
            unit_select = map.matrix[x, y].Object;
        queue.Add(new Search(x, y, map, field));

        while (queue.Count() > 0)
        {
            var next_tile = queue[0];

            BFS_search(next_tile.x, next_tile.y, next_tile.map, next_tile.field);

            queue.Remove(queue[0]);
        }

        return this.path;
    }

    public void BFS_search(int x, int y, Map map, int field)
    {
        if ((x >= 0 && x <= map.GetWidth() - 1) && (y >= 0 && y <= map.GetHeight() - 1)){
            if (map.matrix[x,y].get_type() >= 2)
            {
                if (map.matrix[x, y].visited == false)
                {
                    map.matrix[x,y].visited = true;
                    this.path.Add(map.matrix[x,y]);
                    if (field != 0)
                    {
                        queue.Add(new Search(x + 1, y, map, field - 1));
                        queue.Add(new Search(x - 1, y, map, field - 1));
                            
                        queue.Add(new Search(x, y + 1, map, field - 1));
                        queue.Add(new Search(x, y - 1, map, field - 1));
                    }
                }
            }
        }
    }
}

