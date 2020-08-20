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
            }

            _AddedC.Clear();

            foreach (CellComponent c in _RemoveC)
            {
                _C.Remove(c);
            }
            _RemoveC.Clear();

            //foreach (CellComponent c in _C)
            //{
            //    c.Update(gameTime);
            //}
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

            //Instance Map
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

            //Generate Map
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
                        this.matrix[x,y].height += perlin/noise;
                      
                    }
                }

            }
            
            noiseTex = new Texture2D(width, height);
            pix = new Color[noiseTex.width * noiseTex.height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell cell = this.matrix[i, j];

                    float z= 0.0f;

                    if (cell.height < 0.7f)
                    {
                        z = 0;
                        cell.set_type(0);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0,0,1);
                    }


                    if (cell.height >= 0.7f && cell.height < 0.85f)
                    {
                        cell.set_type(1);
                        z = 0.725f;
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0, 1, 0);
                        //rect = new Rect(8, 1591, 80, 70);
                    }

              
                    if (cell.height >= 0.85f && cell.height < 0.97f)
                    {
                        if(cell.height >= 0.85f && cell.height < 0.90f)
                            z = 0.725f*2;
                        if (cell.height >= 0.90f && cell.height < 0.97f)
                            z = 0.725f*3;
                        cell.set_type(2);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.0f, 0.7f, 0.0f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.height >= 0.97f && cell.height < 1.03f)
                    {
                        if (cell.height >= 0.97f && cell.height < 0.99f)
                            z = 0.725f * 4;
                        if (cell.height >= 0.99f && cell.height < 1.01f)
                            z = 0.725f * 5;
                        if (cell.height >= 1.01f && cell.height < 1.03f)
                            z = 0.725f * 6;
                        cell.set_type(2);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.0f, 0.5f, 0.0f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.height >= 1.03f && cell.height < 1.1f)
                    {
                        if (cell.height >= 1.03f && cell.height < 1.05f)
                            z = 0.725f * 7;
                        if (cell.height >= 1.05f && cell.height < 1.07f)
                            z = 0.725f * 8;
                        if (cell.height >= 1.07f && cell.height < 1.09f)
                            z = 0.725f * 9;
                        if (cell.height >= 1.09f && cell.height < 1.1f)
                            z = 0.725f * 10;

                        cell.set_type(3);
                        pix[(int)j * noiseTex.width + (int)i] = new Color(0.5f, 0.5f, 0.5f);
                        //rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.height >= 1.1f)
                    {
                        pix[(int)j * noiseTex.width + (int)i] = new Color(1, 1, 1);
                        if (cell.height >= 1.1f && cell.height < 1.12f)
                            z = 0.725f * 11;
                        if (cell.height >= 1.12f && cell.height < 1.14f)
                            z = 0.725f * 12;
                        if (cell.height >= 1.14f && cell.height < 1.16f)
                            z = 0.725f * 13;
                        if (cell.height >= 1.16f && cell.height < 1.18f)
                            z = 0.725f * 14;
                        if (cell.height >= 1.18f)
                            z = 0.725f * 15;
                        cell.set_type(3);
                        //rect = new Rect(8, 1178, 80, 70);
                    }

                    cell.position.z = z;
                }

            }

            //Generate Rivers
            //A* algo
            //z < 0

            //Generate Tree
            int counter = 0;
            float[,] bluenoise = new float[height, width];

            //By choosing a different R for each biome we can get a variable density of trees:
       

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    double nx = x / (double)width - 0.5d, ny = y / (double)height - 0.5d;
                    // blue noise is high frequency; try varying this
                    bluenoise[y,x] = Mathf.PerlinNoise((float) nx * 500, (float) ny * 500);
                }
            }

            int R = 1;
            for (int yc = 0; yc < height; yc++)
            {
                for (int xc = 0; xc < width; xc++)
                {
                    Cell cell = this.matrix[xc, yc];
                    if (cell.get_type() == 1 || cell.get_type() == 2)
                    {
                      
                        if (cell.get_type() == 1)
                        {
                            R = 8;
                            if (cell.height >= 0.7f && cell.height < 0.76f)
                                R = 8;
                            if (cell.height >= 0.76f && cell.height < 0.80f)
                                R = 2;
                            if (cell.height >= 0.8f && cell.height < 0.85f)
                                R = 1;
                        }

                        if (cell.get_type() == 2)
                        {
                            if (cell.height >= 0.85f && cell.height < 0.97f)
                                R = 1;
                            if (cell.height >= 0.97f && cell.height < 0.99f)
                                R = 1;
                            if (cell.height >= 0.99f && cell.height < 1.01f)
                                R = 2;
                            if (cell.height >= 1.01f && cell.height < 1.03f)
                                R = 4;
                        }


                        double max = 0;
                        // there are more efficient algorithms than this
                        for (int yn = yc - R; yn <= yc + R; yn++)
                        {
                            for (int xn = xc - R; xn <= xc + R; xn++)
                            {
                                if (0 <= yn && yn < height && 0 <= xn && xn < width)
                                {
                                    double e = bluenoise[yn, xn];
                                    if (e > max)
                                    {
                                        max = e;
                                    }
                                }
                            }
                        }

                        if (bluenoise[yc, xc] == max)
                        {
                            counter++;
                            MTree tree = new MTree(this.matrix[xc, yc], 0);
                            this.matrix[xc, yc].AddComponent(tree);
                            // place tree at xc,yc
                        }
                    }
                }
            }
            Debug.Log("Number Tree: " + counter);

            //Generate Plants

            GameObject.Find("MiniMap").GetComponent<Image>().material.mainTexture = noiseTex;
            // Copy the pixel data to the texture and load it into the GPU.
            noiseTex.SetPixels(pix);
            noiseTex.Apply();
        }

        public void GeneratePlants(MapManager manager)
        {
            for (int i = 0; i < GetHeight(); i++)
            {
                for (int j = 0; j < GetWidth(); j++)
                {
                    if (matrix[i, j].get_type() == 1)
                    {
                       // matrix[i, j].AddComponent(new Plant(matrix[i, j], new Vector2(i, j), new Vector2(1, 0), Color.blue, new Rect(108, 299, 22, 23), manager.tile_flower_blue.texture));
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