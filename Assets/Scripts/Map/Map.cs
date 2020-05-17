using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Map
{
    private int height = 20;
    private int width = 20;

    public Cell[,] matrix;


    private float moy = 0.0f;
    private float lastLevel = 0.0f;

    private float direction = 1;
    private const float HEIGHT_MAX = 5.0f;
    private float counter = 0.0f;



    public float PerlinNoiseMap(int x, int y)
    {
        counter += Time.deltaTime;
        moy = ((float)((int)(Mathf.PerlinNoise(x,y)*10.0f))/10.0f);
        return moy;
    }

    public void Generate(int height, int width, float offset_Z, int noise)
    {
        this.height = height;
        this.width = width;
        Cell[,] matrix = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                matrix[i, j] = new Cell(i, j);
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
                z = Random.Range(3, 6-(int)matrix[x, y].position.z);
            }

            List<Cell> cells = bfs.Research(x, y, z);
            counter += Time.deltaTime;

            foreach (var cell in cells)
            {
                cell.position += new Vector3(0,0, PerlinNoiseMap(0,0));
            }
        }

        //foreach (var cell in matrix)
        //{
        //    cell.position.z /= 5.0f;
        //}


        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (matrix[i, j].position.z < 0.0f)
                    matrix[i, j].set_type(0);

                if (matrix[i, j].position.z >= 0.0f && matrix[i, j].position.z < 2.0f)
                    matrix[i,j].set_type(1);

                if (matrix[i, j].position.z >= 2.0f && matrix[i, j].position.z < 3.5f)
                    matrix[i, j].set_type(2);

                if (matrix[i, j].position.z >= 3.5f && matrix[i, j].position.z <= 15.0f)
                    matrix[i, j].set_type(3);
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
