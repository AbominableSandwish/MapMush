using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;



namespace MapGame
{


    [Serializable]
    public class Map
    {

        private Texture2D noiseTex;
        private Color[] pix;

        private int counterPlant = 0;

        public int height;
        public int width;

        public Cell[,] matrix;


        private float moy = 0.0f;
        private float lastLevel = 0.0f;

        private float direction = 1;
        private const float HEIGHT_MAX = 5.0f;
        private float counter = 0.0f;

        //All Component
        private List<CellComponent> _C = new List<CellComponent>();
        public static List<CellComponent> _AddedC = new List<CellComponent>();
        public static List<CellComponent> _RemoveC = new List<CellComponent>();

        public void Update(float gameTime)
        {
            foreach (CellComponent c in _AddedC)
            {
                _C.Add(c);

                if (c.GetType() == typeof(Plant))
                {
                    counterPlant++;
                }
            }

            _AddedC.Clear();

            foreach (CellComponent c in _RemoveC)
            {
                _C.Remove(c);
            }

            _RemoveC.Clear();

            foreach (CellComponent c in _C)
            {
                c.Update(gameTime);

            }
            Debug.Log("Plants number: " + counterPlant);
        }

        public float PerlinNoiseMap(int x, int y)
        {
            counter += Time.deltaTime;
            moy = ((float) ((int) (Mathf.PerlinNoise(x, y) * 10.0f)) / 10.0f);
            return moy;
        }

        public void Generate(int height, int width, float offset_Z, int noise, int freq, MapManager manager)
        {
            this.height = height;
            this.width = width;


            Cell[,] matrix = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    matrix[i, j] = new Cell(this, i, j);
                    matrix[i, j].position.z = offset_Z;
                  
                }
            }

            this.matrix = matrix;

            for (int i = 0; i < noise; i++)
            {
                int xOrg = Random.Range(1, 5000);

                for (int x = 0; x < this.width; x++)
                {
                    for (int y = 0; y < this.height; y++)
                    {
                        double nx = ((xOrg / 2.0f)+x) / width, ny = ((this.height / 2.0f) + y) / this.height;
                        var perlin =  1* Mathf.PerlinNoise((float)nx*3, (float)ny*3) 
                            + 0.5f * Mathf.PerlinNoise((float)nx * 6, (float)ny * 6)
                            + 0.25f * Mathf.PerlinNoise((float)nx * 12, (float)ny * 12);
                        this.matrix[x,y].position += new Vector3(0, 0, perlin/noise);
                      
                    }
                }

                //Search bfs = new Search();

                //int x = Random.Range(0, this.width);
                //int y = Random.Range(0, this.height);
                //int z;
                //if (matrix[x, y].position.z < 0)
                //{
                //    z = 8;
                //}
                //else
                //{
                //    z = Random.Range(3, 6 - (int) matrix[x, y].position.z);
                //}

                //List<Cell> cells = bfs.Research(x, y, z);
                //counter += Time.deltaTime;

                //foreach (var cell in cells)
                //{
                //    cell.position += new Vector3(0, 0, PerlinNoiseMap(0, 0));
                //}
            }

            // Set up the texture and a Color array to hold pixels during processing.
            noiseTex = new Texture2D(width, height);
            pix = new Color[noiseTex.width * noiseTex.height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell cell = this.matrix[i, j];

                    float z= 0.0f;

                    if (cell.position.z < 0.7f)
                    {
                        z = 0;
                        cell.set_type(0);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0,0,1);
                    }


                    if (cell.position.z >= 0.7f && cell.position.z < 0.85f)
                    {
                        cell.set_type(1);
                        z = 0.725f;
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0, 1, 0);
                        //rect = new Rect(8, 1591, 80, 70);
                    }

              
                    if (cell.position.z >= 0.85f && cell.position.z < 0.97f)
                    {
                        if(cell.position.z >= 0.85f && cell.position.z < 0.90f)
                            z = 0.725f*2;
                        if (cell.position.z >= 0.90f && cell.position.z < 0.97f)
                            z = 0.725f*3;
                        cell.set_type(2);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.0f, 0.7f, 0.0f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.position.z >= 0.97f && cell.position.z < 1.03f)
                    {
                        if (cell.position.z >= 0.97f && cell.position.z < 0.99f)
                            z = 0.725f * 4;
                        if (cell.position.z >= 0.99f && cell.position.z < 1.01f)
                            z = 0.725f * 5;
                        if (cell.position.z >= 1.01f && cell.position.z < 1.03f)
                            z = 0.725f * 6;
                        cell.set_type(2);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.0f, 0.5f, 0.0f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.position.z >= 1.03f && cell.position.z < 1.1f)
                    {
                        if (cell.position.z >= 1.03f && cell.position.z < 1.05f)
                            z = 0.725f * 7;
                        if (cell.position.z >= 1.05f && cell.position.z < 1.07f)
                            z = 0.725f * 8;
                        if (cell.position.z >= 1.07f && cell.position.z < 1.09f)
                            z = 0.725f * 9;
                        if (cell.position.z >= 1.09f && cell.position.z < 1.1f)
                            z = 0.725f * 10;

                        cell.set_type(3);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.5f, 0.5f, 0.5f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.position.z >= 1.1f)
                    {
                        pix[(int)j * noiseTex.width + (int)i] = new Color(1, 1, 1);
                        if (cell.position.z >= 1.1f && cell.position.z < 1.12f)
                            z = 0.725f * 12;
                        if (cell.position.z >= 1.12f && cell.position.z < 1.14f)
                            z = 0.725f * 13;
                        if (cell.position.z >= 1.14f && cell.position.z < 1.16f)
                            z = 0.725f * 14;
                        if (cell.position.z >= 1.16f && cell.position.z < 1.18f)
                            z = 0.725f * 15;
                        if (cell.position.z >= 1.18f)
                            z = 0.725f * 16;
                        cell.set_type(3);
                        //rect = new Rect(8, 1178, 80, 70);
                    }

                    cell.position.z = z;
                }

            }


            GameObject.Find("MiniMap").GetComponent<Image>().material.mainTexture = noiseTex;
            // Copy the pixel data to the texture and load it into the GPU.
            noiseTex.SetPixels(pix);
            noiseTex.Apply();

            //GeneratePlants(manager);
        }

        public void GeneratePlants(MapManager manager)
        {
            for (int i = 0; i < GetHeight(); i++)
            {
                for (int j = 0; j < GetWidth(); j++)
                {
                    if (matrix[i, j].get_type() == 1)
                    {
                        matrix[i, j].AddComponent(new Plant(matrix[i, j], new Vector2(i, j), new Vector2(1, 0), Color.blue, new Rect(108, 299, 22, 23), manager.tile_flower_blue.texture));
                    }
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
                        //manager.GetMap().matrix[i, j].SetDecoration(gTree.GetComponent<SpriteRenderer>());
                    }
                }
            }
        }

        public void AddObject(int x, int y, GameObject Object)
        {
            this.matrix[x, y].Object = Object;
        }

        public int GetHeight()
        {
            return this.height;
        }

        public int GetWidth()
        {
            return this.width;
        }
    }
}