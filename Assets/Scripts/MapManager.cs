using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Slider = UnityEngine.UI.Slider;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MapManager : MonoBehaviour
{
    Map map;

    [SerializeField] private int height = 20;
    [SerializeField] private int width = 20;

    [SerializeField] private int noise;
    [SerializeField] private float offset_Z;


    [SerializeField] private Tilemap map_view;
    [SerializeField] private Tilemap map_level_2;

    [SerializeField] private GameObject prefabCell;

    [SerializeField] private Sprite tile_Water;
    [SerializeField] private Sprite tile_Dirt;
    [SerializeField] private Sprite tile_Rock;
    [SerializeField] private Sprite tile_HightRock;


    public void GenerateMap()
    {
        map_view.ClearAllTiles();
        map_level_2.ClearAllTiles();
        map = new Map();
        map.Generate(height, width, map_view, offset_Z, noise);      
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f / 2)));
        return screenCoord;
    }

    private List<GameObject> objects;

    public void Refresh()
    {
        if (objects != null)
        {
            foreach (var cell in objects)
            {
                Destroy(cell);
            }
        }

        objects = new List<GameObject>();
        
        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int j = 0; j < map.GetWidth(); j++)
            {

                Vector2 positionCell = convertTileCoordInScreenCoord(i, j);
                Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0);
                var cell = Instantiate(prefabCell, this.transform);
                cell.transform.position = positionMap + new Vector3(0, map.matrix[i, j].position.z); ;

                if (map.matrix[i, j].get_type() == 0)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Water;
                }

                if (map.matrix[i, j].get_type() == 1)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Dirt;
                }
                
                if(map.matrix[i, j].get_type() == 2)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Rock;
                }

                if (map.matrix[i, j].get_type() == 3)
                { 
                    cell.GetComponent<SpriteRenderer>().sprite = tile_HightRock;
                }

                cell.GetComponent<SpriteRenderer>().sortingOrder = ((map.GetHeight() - j) + (map.GetWidth() - i)) * 2;
                objects.Add(cell);
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
