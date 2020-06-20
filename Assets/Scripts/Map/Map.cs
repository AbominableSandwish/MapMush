using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;



namespace MapGame
{
    [Serializable]
    public class Map
    {
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

        public void Generate(int height, int width, float offset_Z, int noise, MapManager manager)
        {
            this.height = height;
            this.width = width;
            Cell[,] matrix = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    matrix[i, j] = new Cell(this, i, j);
                    var rdmTmp = Random.Range(0, 100);
                    matrix[i, j].set_type(1);
                    matrix[i, j].position.z = offset_Z;
                }
            }

            this.matrix = matrix;

            for (int i = 0; i < noise * 100; i++)
            {

                Search bfs = new Search();

                int x = Random.Range(0, this.width);
                int y = Random.Range(0, this.height);
                int z;
                if (matrix[x, y].position.z < 0)
                {
                    z = 8;
                }
                else
                {
                    z = Random.Range(3, 6 - (int) matrix[x, y].position.z);
                }

                List<Cell> cells = bfs.Research(x, y, z);
                counter += Time.deltaTime;

                foreach (var cell in cells)
                {
                    cell.position += new Vector3(0, 0, PerlinNoiseMap(0, 0));
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Cell cell = this.matrix[i, j];
                    Texture2D texture = null;
                    Rect rect = new Rect();

                    if (cell.position.z < 0.0f)
                    {
                        cell.set_type(0);
                        texture = manager.tile_Water.texture;
                        rect = new Rect(8, 1045, 80, 55);
                    }

                    if (cell.position.z >= 0.0f && cell.position.z < 2.0f)
                    {
                        cell.set_type(1);
                        texture = manager.tile_Dirt.texture;
                        rect = new Rect(8, 1591, 80, 70);
                    }

                    if (cell.position.z >= 2.0f && cell.position.z < 3.5f)
                    {
                        cell.set_type(2);
                        texture = manager.tile_Rock.texture;
                        rect = new Rect(8, 1398, 80, 70);
                    }

                    if (cell.position.z >= 3.5f && cell.position.z <= 15.0f)
                    {
                        cell.set_type(3);
                        texture = manager.tile_HightRock.texture;
                        rect = new Rect(8, 1178, 80, 70);
                    }

                    cell.AddComponent(new Graphic(cell, new ObjectTransform(new Vector2((int)cell.position.x, (int)cell.position.y)), rect,texture));
                }
              
            }

            GeneratePlants(manager);
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
                    //float rdm = Random.Range(0.0f, 1.0f);
                    //if (rdm < 0.994f && rdm > 0.95)
                    //{
                       
                    //}

                    //if (rdm > 0.994f)
                    //{

                    //    ////TREE
                    //    //GameObject gTree = Instantiate(tree, cell.transform);
                    //    //gTree.GetComponent<SpriteRenderer>().sortingOrder =
                    //    //    cell.GetComponent<SpriteRenderer>().sortingOrder + 5;
                    //    //gTree.GetComponentsInChildren<SpriteRenderer>()[1].sortingOrder =
                    //    //    gTree.GetComponent<SpriteRenderer>().sortingOrder;
                    //    //map.matrix[i, j].SetDecoration(gTree.GetComponent<SpriteRenderer>());
                    //}
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