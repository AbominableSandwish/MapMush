using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

class Map
{
    private int height = 20;
    private int width = 20;

    public Cell[,] matrix;


    public void Generate(Tilemap map_view)
    {
        string msg = "";
        Cell[,] matrix = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                matrix[i, j] = new Cell(i, j);
                var rdmTmp = Random.Range(0, 100);

                if (rdmTmp <= 5)
                {

                    //var bite = randi()%100+0
                    matrix[i, j].set_type(1);

                    matrix[i, j].set_texture(1);

                    msg += "1, ";
                }
                else
                {
                    matrix[i, j].set_type(2);

                    matrix[i, j].set_texture(0);

                    msg += "2, ";
                }

            }

            msg += "\n";
        }

        Debug.Log("++   Map Ready   ++");

        Debug.Log("++   Height: " + height.ToString() + ", Width: " + width.ToString() + "   ++");

        Debug.Log(msg);

        this.matrix = matrix;
    }

    public void AddObject(int x, int y, int contain)
    {
        this.matrix[x, y].contain = contain;
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
