using System.Collections.Generic;
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

    public Mode cameraMode = Mode.FREE;

    private HashPartition hash;

    private Map map;
    [SerializeField] private int max_bucket;
    private Bucket[,] buckets;

    private Vector2 direction;

    [SerializeField] private float LengthVisibility;

    public void RestartCamera()
    {
        hash = new HashPartition();
        hash.cutMap(max_bucket, GameObject.Find("Map").GetComponent<MapManager>().GetMap());
        ShowBucket();
        distance = 3.5f;
    }

    public void ShowBucket()
    {
        buckets = this.hash.GetBuckets();

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

    void FixedUpdate()
    {
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
            transform.position + new Vector3(direction.x, direction.y) * Time.deltaTime * Velocity * acceleration, 1.0f);
        distance += (transform.position - newPosition).magnitude;
        transform.position = newPosition;

        if (distance >= 3.5f)
        {
            distance = 0.0f;
            Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);

            for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
            {
                for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
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
                    int index_x = (int)(x / Mathf.Sqrt(max_bucket));
                    int index_y = (int)(y / Mathf.Sqrt(max_bucket));

                    if (this.buckets[index_x, index_y] == null)
                        this.buckets[index_x, index_y] = new Bucket();
                    this.buckets[index_x, index_y].cells.Add(map.matrix[x, y]);

                }
            }

            for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
            {
                for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
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

        public Vector3 convertTileCoordInScreenCoord(Vector3 position)
        {
            Vector3 screenCoord = new Vector3();
            screenCoord.x = (float)(-0.25f + ((position.x - position.y)));
            screenCoord.y = (float)(-4.80f + ((position.x + position.y) * (0.5f)));
            return screenCoord;
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
