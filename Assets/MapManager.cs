using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;

public class MapManager : MonoBehaviour
{
    Map map;

    public TileBase tileGreen;
    public TileBase tileBrown;

    [SerializeField] private Tilemap map_view;

    public void GenerateMap()
    {
        map = new Map();
        map.Generate(map_view);      
    }

    public void Refresh()
    {
        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int j = 0; j < map.GetWidth(); j++)
            {
                if (map.matrix[i, j].get_type() == 1)
                {
                    map_view.SetTile(new Vector3Int(i, j, 0), tileBrown);
                    
                }
                else
                {
                    map_view.SetTile(new Vector3Int(i, j, 0), tileGreen);
                }
            }
        }
    }

    public Map GetMap()
    {
        return this.map;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
