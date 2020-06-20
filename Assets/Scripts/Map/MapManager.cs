using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography;
using MapGame;
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
    [SerializeField] private int nbrOfTree;

    [Header("Prefab & Sprites")]
    [SerializeField] private GameObject prefabCell;
    [SerializeField] public Sprite tile_Water;
    [SerializeField] public Sprite tile_Dirt;
    [SerializeField] public Sprite tile_Rock;
    [SerializeField] public Sprite tile_HightRock;

    [Header("Objects")] [SerializeField] private GameObject tree;

    [Header("Decoration")] [SerializeField]
    private GameObject prefab_deco;
    [SerializeField] private Sprite tile_flower_red;
    [SerializeField] public Sprite tile_flower_blue;
    [SerializeField] private Sprite tile_flower_white;

    [SerializeField] private Sprite water;

    [SerializeField] private bool viewIsOn = false;

    public void GenerateMap()
    {
        map = new Map();
        map.Generate(height, width, offset_Z, noise, this);
        if (viewIsOn)
        {
            Clean();
        }

        SaveIntoJson();
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    public void SelectCell(Vector2 position, Color color)
    {
       // if(map)
        map.matrix[(int)position.x, (int) position.y].render.color = color;
    }

    public void Update()
    {
        map.Update(Time.deltaTime);
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
                Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(0, 0);
                var cell = Instantiate(prefabCell, this.transform);
                cell.transform.position = positionMap + new Vector3(0, map.matrix[i, j].position.z);
                cell.GetComponent<SpriteRenderer>().sortingOrder = ((map.GetHeight() - j) + (map.GetWidth() - i)) * 5;

                if (map.matrix[i, j].get_type() == 0)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Water;
                    cell.transform.position = positionMap;

                }

                if (map.matrix[i, j].get_type() == 1)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Dirt;
                    float color = +0.9f - 0.1f * (shadow / map.matrix[i, j].position.z / 2);
                    cell.GetComponent<SpriteRenderer>().color = new Color(color, color, color, 1);

                    float rdm = Random.Range(0.0f, 1.0f);
                    if (rdm < 0.994f && rdm > 0.95)
                    {

                    }

                    if (rdm > 0.994f)
                    {
                        ////TREE
                        //GameObject gTree = Instantiate(tree, cell.transform);
                        //gTree.GetComponent<SpriteRenderer>().sortingOrder =
                        //    cell.GetComponent<SpriteRenderer>().sortingOrder + 5;
                        //gTree.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder =
                        //    gTree.GetComponent<SpriteRenderer>().sortingOrder;
                        //map.matrix[i, j].SetDecoration(gTree.GetComponent<SpriteRenderer>());
                    }
                }

                if (map.matrix[i, j].get_type() == 2)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Rock;
                    float color = +0.9f - 0.1f * (shadow / map.matrix[i, j].position.z / 2);
                    cell.GetComponent<SpriteRenderer>().color = new Color(color, color, color, 1);
                }

                if (map.matrix[i, j].get_type() == 3)
                {
                    cell.GetComponent<SpriteRenderer>().sprite = tile_HightRock;
                }

                if (map.matrix[i, j].render == null)
                    map.matrix[i, j].SetSpriteRender(cell.GetComponent<SpriteRenderer>());

                objects.Add(cell);
            }
        }

        for (int i = 0; i < map.GetWidth(); i++)
        {
            for (int nbr = 1; nbr < 6; nbr++)
            {
                if (map.matrix[i, 0].get_type() != 0)
                {
                    Vector2 positionCell = convertTileCoordInScreenCoord(i, 0);
                    Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(0, -0.38f * nbr);
                    var cell = Instantiate(prefabCell, this.transform);
                    cell.transform.position = positionMap + new Vector3(0, map.matrix[i, 0].position.z);
                    cell.GetComponent<SpriteRenderer>().sortingOrder = map.matrix[i, 0].render.sortingOrder - 1 - nbr;
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Rock;
                    cell.GetComponent<SpriteRenderer>().color = map.matrix[i, 0].render.color;
                }
                else
                {
                    Vector2 positionCell = convertTileCoordInScreenCoord(i, 0);
                    Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(+0.5f, -0.38f * nbr);
                    var cell = Instantiate(prefabCell, this.transform);
                    cell.transform.position = positionMap + new Vector3(0, map.matrix[i, 0].position.z);
                    cell.GetComponent<SpriteRenderer>().sortingOrder = map.matrix[i, 0].render.sortingOrder + 2 - nbr;
                    cell.GetComponent<SpriteRenderer>().sprite = water;
                    cell.GetComponent<SpriteRenderer>().flipX = true;
                }

            }
        }

        for (int j = 0; j < map.GetWidth(); j++)
        {
            for (int nbr = 1; nbr < 6; nbr++)
            {
                if (map.matrix[0, j].get_type() != 0)
                {

                    Vector2 positionCell = convertTileCoordInScreenCoord(0, j);
                    Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(0, -0.38f * nbr);
                    var cell = Instantiate(prefabCell, this.transform);
                    cell.transform.position = positionMap + new Vector3(0, map.matrix[0, j].position.z);
                    cell.GetComponent<SpriteRenderer>().sortingOrder = map.matrix[0, j].render.sortingOrder - 1 - nbr;
                    cell.GetComponent<SpriteRenderer>().sprite = tile_Rock;
                    cell.GetComponent<SpriteRenderer>().color = map.matrix[0, j].render.color;

                }
                else
                {
                    Vector2 positionCell = convertTileCoordInScreenCoord(0, j);
                    Vector3 positionMap = new Vector3(positionCell.x, positionCell.y, 0) + new Vector3(-0.5f, -0.38f *nbr);
                    var cell = Instantiate(prefabCell, this.transform);
                    cell.transform.position = positionMap + new Vector3(0, map.matrix[0, j].position.z);
                    cell.GetComponent<SpriteRenderer>().sortingOrder = map.matrix[0, j].render.sortingOrder + 2 - nbr;
                    cell.GetComponent<SpriteRenderer>().sprite = water;
                }
            }
        }
    }

    [SerializeField] private float shadow;

    public Map GetMap()
    {
        return this.map;
    }

    public void SaveIntoJson()
    {
        string map = ToJson(this.map.matrix);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/MapData.json", map);
    }

    private void ReadFormatJson()
    {
        string path = Application.dataPath + "/ScoreRecords.json";
        string jsonString = File.ReadAllText(path);
    }


    [System.Serializable]
    private class MapDictionary
    {
        public Cell[] items;
    }

    private static string ToJson(Cell[,] cells)
    {
        List<Cell> dictionaryItemsList = new List<Cell>();
        foreach (var cell in cells)
        {
            dictionaryItemsList.Add(cell);
        }

        MapDictionary dictionaryArray = new MapDictionary();
        dictionaryArray.items = dictionaryItemsList.ToArray();

        return JsonUtility.ToJson(dictionaryArray);
    }

}
