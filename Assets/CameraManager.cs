using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraManager : MonoBehaviour
{
    private HashPartition hash;
    
    private Map map;
    private int max_bucket = 256;

    public void RestartCamera()
    {
        hash = new HashPartition();
        hash.cutMap(max_bucket ,GameObject.Find("Map").GetComponent<MapManager>().GetMap());
        ShowBucket();
    }

    public void ShowBucket()
    {
        Bucket[,] buckets=this.hash.GetBuckets();
 
        for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
        {
            for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
            {

                    foreach (var cell in buckets[x, y].cells)
                    {
                        cell.render.gameObject.SetActive(buckets[x, y].IsVisible);
                    }
            }
        }

        foreach (var cell in buckets[0, 0].cells)
        {
            cell.render.gameObject.SetActive(true);
        }
        
    }

    public class Bucket
    {
        public Vector2 position;
        public List<Cell> cells;

        public bool IsVisible;

        public Bucket()
        {
            this.cells = new List<Cell>();
            this.IsVisible = false;
        }
    }

    class HashPartition
    {
       

        private Bucket[,] buckets;

        public Bucket[,] GetBuckets()
        {
            return this.buckets;
        }

        public Bucket[,] cutMap(int max_bucket, Map map)
        {
            this.buckets = new Bucket[(int)(map.GetWidth() / Mathf.Sqrt(max_bucket)), (int)(map.GetHeight() / Mathf.Sqrt(max_bucket))];

            for (int x = 0; x < map.GetWidth(); x++)
            {
                for (int y = 0; y < map.GetHeight(); y++)
                {
                    int index_x = (int) (x / Mathf.Sqrt(max_bucket));
                    int index_y = (int) (y / Mathf.Sqrt(max_bucket));
                   
                    if (this.buckets[index_x, index_y] == null)
                        this.buckets[index_x, index_y] = new Bucket();
                    this.buckets[index_x, index_y].cells.Add(map.matrix[x,y]);

                }
            }
            return this.buckets;
        }

     
   
    }
}
