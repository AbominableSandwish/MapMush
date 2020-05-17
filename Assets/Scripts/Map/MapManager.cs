using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
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
    private Map map;
    private List<GameObject> objects;

    [Header("Size Map")]
    [SerializeField] private int height;
    [SerializeField] private int width;

    [Header("Generation Procedural")]
    [SerializeField] private int noise;
    [SerializeField] private float offset_Z;

    [Header("Prefab & Sprites")]
    [SerializeField] private GameObject prefabCell;
    [SerializeField] private Sprite tile_Water;
    [SerializeField] private Sprite tile_Dirt;
    [SerializeField] private Sprite tile_Rock;
    [SerializeField] private Sprite tile_HightRock;

    public void GenerateMap()
    {
        map = new Map();
        map.Generate(height, width, offset_Z, noise);
        Clean();
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    public void Clean()
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
                Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0)+ new Vector3(0, 0);
                var cell = Instantiate(prefabCell, this.transform);
                cell.transform.position = positionMap + new Vector3(0, map.matrix[i, j].position.z);
                
                if (map.matrix[i, j].get_type() == 0)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Water;
                }

                if (map.matrix[i, j].get_type() == 1)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Dirt;
                    float color = +0.9f - 0.1f * (shadow / map.matrix[i, j].position.z/2);
                    cell.GetComponent<SpriteRenderer>().color = new Color(color,color,color,1);
                }
                
                if(map.matrix[i, j].get_type() == 2)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Rock;
                    float color = +0.9f - 0.1f * (shadow / map.matrix[i, j].position.z/2);
                    cell.GetComponent<SpriteRenderer>().color = new Color(color, color, color, 1);
                }

                if (map.matrix[i, j].get_type() == 3)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_HightRock;
                }

                if (map.matrix[i, j].render == null)
                    map.matrix[i, j].SetSpriteRender(cell.GetComponent<SpriteRenderer>());

                cell.GetComponent<SpriteRenderer>().sortingOrder = ((map.GetHeight() - j) + (map.GetWidth() - i)) * 3;
                objects.Add(cell);
            }
        }
    }

    [SerializeField] private float shadow;

    public Map GetMap()
    {
        return this.map;
    }
}
