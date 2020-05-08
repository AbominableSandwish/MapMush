using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraManager : MonoBehaviour
{
    private HashPartition hash;
    
    private Map map;
    private int max_bucket = 256;
    private Bucket[,] buckets;

    public void RestartCamera()
    {
        hash = new HashPartition();
        hash.cutMap(max_bucket ,GameObject.Find("Map").GetComponent<MapManager>().GetMap());
        ShowBucket();
    }

    public void ShowBucket()
    {
        buckets=this.hash.GetBuckets();
 
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
        public Vector3 position;
        public List<Cell> cells;

        public bool IsVisible;

        public Bucket()
        {
            this.cells = new List<Cell>();
            this.IsVisible = false;
            this.position = new Vector3();
        }

        public void SetVisibilty(bool isVisible)
        {
            this.IsVisible = isVisible;
            foreach (var cell in cells)
            {
                cell.render.gameObject.SetActive(this.IsVisible);
            }
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

            for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
            {
                for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
                {
                    Vector3 positionBucket= new Vector3();
                    foreach (var cell in buckets[x, y].cells)
                    {
                        positionBucket += convertTileCoordInScreenCoord(cell.position);
                    }

                    positionBucket /= buckets[x, y].cells.Count;
                    buckets[x, y].position = positionBucket;
                }
            }
            return this.buckets;
        }

        public Vector3 convertTileCoordInScreenCoord(Vector3 position)
        {
            Vector3 screenCoord = new Vector3();
            screenCoord.x = (float)(-0.25f + ((position.x - position.y) * 0.5f));
            screenCoord.y = (float)(-4.80f + ((position.x + position.y) * (0.5f / 2)));
            return screenCoord;
        }

    }



    void OnDrawGizmos()
    {
        Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);
        for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
        {
            for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
            {

                if (((this.buckets[x, y].position - positionCamera).magnitude <= 17.0f) &&
                    (this.buckets[x, y].position - positionCamera).magnitude >= -17.0f)
                {
                    Gizmos.color = Color.green;
                    this.buckets[x, y].SetVisibilty(true);
                }
                else
                {
                    Gizmos.color = Color.gray;
                    this.buckets[x, y].SetVisibilty(false);
                }

                Gizmos.DrawSphere(this.buckets[x,y].position, 1.0f);
            }
        }

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(positionCamera, 2.0f);
    }
  
}
