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

    [SerializeField] private int height = 20;
    [SerializeField] private int width = 20;
    [SerializeField] private int Level = 2;

    public TileBase tileGreen;
    public TileBase tileBrown;
    public TileBase tileLevel2;

    [SerializeField] private Tilemap map_view;
    [SerializeField] private Tilemap map_level_2;

    public void GenerateMap()
    {
        map_view.ClearAllTiles();
        map_level_2.ClearAllTiles();
        map = new Map();
        map.Generate(height, width, map_view);      
    }

    public void Refresh()
    {
        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int j = 0; j < map.GetWidth(); j++)
            {
                if (map.matrix[i, j].get_type() == 1)
                {
                    
                    if (map.matrix[i, j].Level == 2)
                    {
                        map_level_2.SetTile(new Vector3Int(i, j, 0), tileLevel2);
                    }
                    else
                    {
                        map_view.SetTile(new Vector3Int(i, j, 0), tileBrown);
                    }
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
