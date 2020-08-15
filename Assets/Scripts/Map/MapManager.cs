using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Security.Cryptography;
using JetBrains.Annotations;
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
    [SerializeField] private int noise = 1;
    [SerializeField] private int freq = 1;
    [SerializeField] private float offset_Z = 0;
    //[SerializeField] private int nbrOfTree = 0;

    [Header("Prefab & Sprites")]

    [SerializeField] public Material material;
    [SerializeField] public GameObject prefabCell;
    [SerializeField] public Sprite tile_Water;
    [SerializeField] public Sprite tile_Dirt;
    [SerializeField] public Sprite tile_Rock;
    [SerializeField] public Sprite tile_HightRock;

    [Header("Objects")] [SerializeField] private GameObject tree;

    [Header("Decoration")] [SerializeField]
    private GameObject prefab_deco;
    [SerializeField] public Sprite tile_flower_red;
    [SerializeField] public Sprite tile_flower_blue;
    [SerializeField] public Sprite tile_flower_white;

    [SerializeField] private Sprite water;

    //[SerializeField] private bool viewIsOn = false;

    private CameraManager cameraManager;

    public void GenerateMap(CameraManager cameraManager)
    {
        this.cameraManager = cameraManager;
        map = new Map();
        map.Generate(height, width, offset_Z, noise, freq, this);
        SaveIntoJson();

        nextTime = Time.time + 20;
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    //public void SelectCell(Vector2 position, Color color)
    //{
    //   // if(map)
    //    map.matrix[(int)position.x, (int) position.y].color = color;
    //}

    enum  GameTime
    {
        DAY,
        NIGHT
    }

    private float nextTime;

    private GameTime gameTime = GameTime.DAY;
    

    public void Update()
    {
        if (map != null)
        {
            float time = Time.deltaTime;
            map.Update(time);
            Material water;

            switch (gameTime)
            {
                case GameTime.DAY:
                    if (Time.time >= nextTime)
                    {
                        nextTime += 20;
                        gameTime = GameTime.NIGHT;
                    }

                    if (material.color.r < 1.0f)
                    {
                        material.color += new Color(1.0f, 1.0f, 1.0f, 0.0f) * time / 20.0f;
                        water = GameObject.Find("RenderWater").GetComponent<SpriteRenderer>().material;
                        water.SetColor("_WaterColor", water.GetColor("_WaterColor") + Color.white * time / 25.0f);
                        water.SetColor("_LightFoamColor", water.GetColor("_LightFoamColor") + Color.white * time / 25.0f);
                        water.SetColor("_DarkFoamColor", water.GetColor("_DarkFoamColor") + Color.white * time / 25.0f);

                    }
                   
                    if (Camera.main.backgroundColor.b < 1.0f)
                    {
                        Camera.main.backgroundColor += new Color(0.5f, 0.5f, 1.0f, 0.0f) * time / 20.0f;
                    }
                  
                    break;
                case GameTime.NIGHT:
                    if (Time.time >= nextTime)
                    {
                        nextTime += 20;
                        gameTime = GameTime.DAY;
                    }


                    if (material.color.r > 0.2f)
                    {
                        material.color -= new Color(1.0f, 1.0f, 1.0f, 0.0f) * time / 20.0f;
                        water = GameObject.Find("RenderWater").GetComponent<SpriteRenderer>().material;
                        //if(water.GetColor("_WaterColor").r > 0.2f)
                     
                        if (material.color.r >= 0.6f)
                        {
                            water.SetColor("_WaterColor", water.GetColor("_WaterColor") - new Color(0.0f, 0.5f, 0.5f) * time / 25.0f);
                            water.SetColor("_WaterColor", water.GetColor("_WaterColor") + new Color(0.5f, 0.0f, 0.0f) * time / 25.0f);
                        }
                        else
                        {
                            water.SetColor("_WaterColor", water.GetColor("_WaterColor") - new Color(0.5f, 0.5f, 0.5f, 0) * time / 25.0f);
                        }

                        water.SetColor("_LightFoamColor", water.GetColor("_LightFoamColor") - Color.white * time / 25.0f);
                        water.SetColor("_DarkFoamColor", water.GetColor("_DarkFoamColor") - Color.white * time / 25.0f);
                    }

                    if (Camera.main.backgroundColor.b > 0.22f)
                    {
                        Camera.main.backgroundColor -= new Color(0.5f, 0.5f, 1.0f, 0.0f) * time / 20.0f;
                    }

               
                    break;
            }
           
        }

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
