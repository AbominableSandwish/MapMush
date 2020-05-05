using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

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

    [SerializeField] private GameObject prefabCell;

    [SerializeField] private Sprite tileRock;


    public void GenerateMap()
    {
        map_view.ClearAllTiles();
        map_level_2.ClearAllTiles();
        map = new Map();
        map.Generate(height, width, map_view);      
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f / 2)));
        return screenCoord;
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
                       // map_level_2.SetTile(new Vector3Int(i, j, 0), tileLevel2);
                        Vector2 positionCell = convertTileCoordInScreenCoord(i, j);
                        Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0);
                        var cell = Instantiate(prefabCell, this.transform);
                        cell.transform.position = positionMap;
                        cell.GetComponent<SpriteRenderer>().sprite = tileRock;
                        cell.GetComponent<SpriteRenderer>().sortingOrder =
                            ((map.GetHeight() - j) + (map.GetWidth() - i)) * 2;
                    }
                    else
                    {
                        
                        Vector2 positionCell = convertTileCoordInScreenCoord(i, j);
                        Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0);
                        var cell = Instantiate(prefabCell, this.transform);
                        cell.transform.position = positionMap;
                        cell.GetComponent<SpriteRenderer>().sprite = tileRock;
                        cell.GetComponent<SpriteRenderer>().sortingOrder =
                            ((map.GetHeight() - j) + (map.GetWidth() - i)) * 2;
                        cell.GetComponent<HauteurScript>().SetFly();
                        //map_view.SetTile(new Vector3Int(i, j, 0), tileBrown);
                    }
                }
                else
                {
                   // map_level_2.SetTile(new Vector3Int(i, j, 0), tileLevel2);
                    Vector2 positionCell = convertTileCoordInScreenCoord(i, j);
                    Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0);
                    var cell = Instantiate(prefabCell, this.transform);
                    cell.transform.position = positionMap;
                    cell.GetComponent<SpriteRenderer>().sortingOrder = ((map.GetHeight() - j) + (map.GetWidth() - i))*2;

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
