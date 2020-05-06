using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map
{
    private int height = 20;
    private int width = 20;

    public Cell[,] matrix;


    private float moy;

    public float PerlinNoiseMap(float level)
    {

        if (moy == null)
        {
            moy = level + Random.Range(-1.0f, 1.0f);
        }
        else
        {
            moy += Random.Range(-0.5f, 0.5f)/2;
            if (moy > 0.5f)
            {
                moy = 0.5f;
            }
            if (moy < -0.5f)
            {
                moy = -0.5f;
            }
        }

        return level + moy/8;
    }

    public void Generate(int height, int width,Tilemap map_view)
    {
        this.height = height;
        this.width = width;

        string msg = "";
        Cell[,] matrix = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                matrix[i, j] = new Cell(i, j);
                var rdmTmp = Random.Range(0, 100);
                matrix[i, j].set_type(1);
                matrix[i, j].SetLevel(1);

                //        matrix[i, j].set_texture(0);
                //    if (rdmTmp <= 8)
                //    {

                //        //var bite = randi()%100+0
                //        matrix[i, j].set_type(1);

                //        matrix[i, j].set_texture(1);

                //        if (Random.Range(0, 100) <= 50)
                //        {
                //            matrix[i, j].SetLevel(2);
                //        }

                //        msg += "1, ";
                //    }
                //    else
                //    {
                //        matrix[i, j].set_type(2);

                //        matrix[i, j].set_texture(0);

                //        msg += "2, ";
                //    }

            }

            msg += "\n";
        }

        Debug.Log("++   Map Ready   ++");

        Debug.Log("++   Height: " + height.ToString() + ", Width: " + width.ToString() + "   ++");

        //Debug.Log(msg);

        this.matrix = matrix;

        for (int i = 0; i < 500; i++)
        {
            float tmp = Random.Range(-0.5f, 0.5f);
            Search bfs = new Search();
            List<Cell> cells = bfs.Research(Random.Range(0, width), Random.Range(0, this.height), 10);
                //bfs.Research(width / 2, height / 2, 10);

            foreach (var cell in cells)
            {
                cell.hauteur = PerlinNoiseMap(cell.hauteur+ tmp);
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
