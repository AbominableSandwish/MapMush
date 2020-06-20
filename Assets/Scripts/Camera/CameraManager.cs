using System.Collections.Generic;
using MapGame;
using UnityEngine;
using UnityEngine.UIElements;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    public enum Mode
    {
        FREE,
        TARGET,
        KINEMATIC,
        STATIC
    }

    public static Vector3 convertTileCoordInScreenCoord(Vector3 position)
    {
        Vector3 screenCoord = new Vector3();
        screenCoord.x = (float)(((position.x - position.y)));
        screenCoord.y = (float)(((position.x + position.y) * (0.5f)));
        return screenCoord;
    }

    public Mode cameraMode = Mode.FREE;

    private HashPartition hash;

    public bool IsOn = false;
    private Map map;
    [SerializeField] private int max_bucket;
    private Bucket[,] buckets;

    private Vector2 direction;

    [SerializeField] private float LengthVisibility;

    public void ResetView()
    {
       
        buckets = this.hash.GetBuckets();

        for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
        {
            for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
            {

                foreach (var cell in buckets[x, y].cells)
                {
                    buckets[x, y].SetVisibilty(true);
                    //cell.render.gameObject.SetActive(true);
                }
            }
        }

        hash = null;
    }

    public void RestartCamera()
    {
        if (IsOn)
        {
            hash = new HashPartition();
            hash.cutMap(max_bucket, GameObject.Find("Map").GetComponent<MapManager>().GetMap());
            ShowBucket();
            distance = 3.5f;
        }
    }

    public void ShowBucket()
    {
        buckets = this.hash.GetBuckets();

        for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
        {
            for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
            {

                foreach (var cell in buckets[x, y].cells)
                {
                    cell.render.gameObject.SetActive(buckets[x, y].IsVisible);
                }
            }
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
                cell.SetIsVisible(this.IsVisible);
            }
        }


    }

    [SerializeField] private float Velocity = 5000f;

    private float distance;

    private Vector2 lastDirection;
    private float acceleration = 0;

    public void CameraStop()
    {
        acceleration = 0.0f;
        lastDirection = Vector3.zero;
    }

    public void SetDirectionX(int x)
    {
        this.direction = new Vector2(x, this.direction.y);
    }

    public void SetDirectionY(int y)
    {
        this.direction = new Vector2(this.direction.x, y);
    }

    public void SetDirectionX(float x)
    {
        this.direction = new Vector2(x, this.direction.y);
    }

    public void SetDirectionY(float y)
    {
        this.direction = new Vector2(this.direction.x, y);
    }

    void FixedUpdate()
    {
        if (!IsOn)
        {
            if(hash != null)
                ResetView();
        }

        if (direction.magnitude != 0.0f)
        {
            lastDirection = direction;
            if (acceleration < 1.0f)
            {
                acceleration += Time.deltaTime * 2.0f;
                if (acceleration > 1.0f)
                {
                    acceleration = 1.0f;
                }
            }

            lastDirection = direction;
        }
        else
        {
            if (acceleration > 0)
            {
                acceleration -= Time.deltaTime*3;
                Vector3 nextposition = Vector3.Lerp(transform.position,
                    transform.position += new Vector3(lastDirection.x, lastDirection.y) * Time.deltaTime * Velocity *
                                          acceleration, 1.0f);
                transform.position = nextposition;
            }
            else
            {
                acceleration = 0.0f;
            }

            //lastDirection = direction;
        }

        Vector3 newPosition = Vector3.Lerp(transform.position,
            transform.position + new Vector3(direction.x, direction.y) * Time.deltaTime * Velocity * acceleration,
            1.0f);
        distance += (transform.position - newPosition).magnitude;
        transform.position = newPosition;

        if (IsOn)
        {
            if (hash == null)
            {
                RestartCamera();
                return;
            }

            if (distance >= 3.5f)
            {
                distance = 0.0f;
                Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);

                for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
                {
                    for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
                    {

                        if ((this.buckets[x, y].position - positionCamera).magnitude <= LengthVisibility &&
                            (this.buckets[x, y].position - positionCamera).magnitude >= -LengthVisibility)
                        {
                            this.buckets[x, y].SetVisibilty(true);
                        }
                        else
                        {
                            this.buckets[x, y].SetVisibilty(false);
                        }
                    }
                }
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
            int area = (int)Mathf.Sqrt(max_bucket);

            this.buckets = new Bucket[area, area];
            for (int j = 0; j < area; j++)
            {
                for (int i = 0; i < area; i++)
                {
                    int height = map.GetHeight();
                    int width = map.GetWidth();

                    for (int y = 0 + (height / area) * j; y < height / area * (j+1); y++)
                    {
                        for (int x = 0 + (width / area) * i; x < width / area * (i+1); x++)
                        {
                            int index_x = (int)(x / (width / area));
                            int index_y = (int)(y / (height/ area));


                            if (this.buckets[j, i] == null)
                                this.buckets[j, i] = new Bucket();
                            this.buckets[j, i].cells.Add(map.matrix[x, y]);
                        }
                    }
                }
            }
            //int width = map.GetWidth();
            //for (int x = 0; x < width; x++)
            //{
            //    int height = map.GetHeight();
            //    for (int y = 0; y < height; y++)
            //    {
            //        int index_x = (int)(x / (width/ Mathf.Sqrt(max_bucket)));
            //        int index_y = (int)(y / (height/ Mathf.Sqrt(max_bucket)));

            //        if (index_x == max_bucket && index_y == max_bucket)
            //        {
            //            Debug.Log("Here");
            //        }

            //        if (this.buckets[index_x, index_y] == null)
            //            this.buckets[index_x, index_y] = new Bucket();
            //        this.buckets[index_x, index_y].cells.Add(map.matrix[x, y]);

            //    }
            //}

            for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
            {
                for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
                {
                    Vector3 positionBucket = new Vector3();
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

       

    }



    //void OnDrawGizmos()
    //{
    //    if (this.buckets != null)
    //    {
    //        Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);
    //        for (int x = 0; x < (int) (Mathf.Sqrt(max_bucket)); x++)
    //        {
    //            for (int y = 0; y < (int) (Mathf.Sqrt(max_bucket)); y++)
    //            {

    //                if ((this.buckets[x, y].position - positionCamera).magnitude <= 19f &&
    //                    (this.buckets[x, y].position - positionCamera).magnitude >= -19f)
    //                {
    //                    Gizmos.color = Color.green;
    //                    this.buckets[x, y].SetVisibilty(true);
    //                }
    //                else
    //                {
    //                    Gizmos.color = Color.gray;
    //                    this.buckets[x, y].SetVisibilty(false);
    //                }

    //                Gizmos.DrawSphere(this.buckets[x, y].position, 1.0f);
    //            }
    //        }

    //        Gizmos.color = Color.black;
    //        Gizmos.DrawSphere(positionCamera, 2.0f);
    //    }
    //}

}
