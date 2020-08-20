using System.Collections;
using System.Collections.Generic;
using MapGame;
using UnityEngine;
using UnityEngine.UIElements;
using Tree = UnityEngine.Tree;
using Vector3 = UnityEngine.Vector3;

public class CameraManager : MonoBehaviour
{
    private Camera camera;
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

    public bool IsOn = true;
    private Map map;
    [SerializeField] private int max_bucket = 1536;
    private Bucket[,] buckets;

    public Queue<Bucket> bucketsToLoad;
    public Queue<Bucket> bucketsToDeload;

    public List<Bucket> bucketsVîsible;

    private Vector2 direction = Vector2.zero;

    [SerializeField] private float LengthVisibility = 30.0f;

    public void ResetView()
    {
        camera = GetComponent<Camera>();

        bucketsVîsible = new List<Bucket>();
        buckets = this.hash.GetBuckets();

        for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
        {
            for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
            {
                foreach (var cell in buckets[x, y].cells)
                {
                    buckets[x, y].SetVisibilty(false);
                    //cell.render.gameObject.SetActive(true);
                }
            }
        }

        hash = null;
    }

    public BufferGraphic buffer;
    public void RestartCamera(BufferGraphic buffer)

    {
        bucketsVîsible = new List<Bucket>();
        bucketsToLoad = new Queue<Bucket>();
        bucketsToDeload = new Queue<Bucket>();
        if (IsOn)
        {
            this.buffer = buffer;
            hash = new HashPartition();
            
            hash.cutMap(buffer, max_bucket, GameObject.Find("Map").GetComponent<MapManager>().GetMap());
            
            ShowBucket();
            distance = 3.5f;
        }

        StartCoroutine(Check());
    }

    private IEnumerator Check()
    {

        if (bucketsToDeload.Count > 0)
        {
            Bucket bucket = bucketsToDeload.Dequeue();
            foreach (var cell in bucket.cells)
            {
                Graphic render = cell.GetComponent<Graphic>();
                if (render != null)
                    render.Remove();
                Cell.Destroy(render);
            }
        }

        if (bucketsToLoad.Count > 0)
        {
            var bucket = bucketsToLoad.Dequeue();

            foreach (var cell in bucket.cells)
            {
                Graphic render = new Graphic(buffer, cell);


                MTree tree = cell.GetComponent<MTree>();
                if (tree != null)
                {
                    render.AddObject(BufferGraphic.TypeTexture.Tree, tree.type);
                }
                cell.AddComponent(render);
            }
        }

     

        yield return new WaitForSeconds(0.2f);
    }

    public void ShowBucket()
    {
        buckets = this.hash.GetBuckets();

        if (buckets != null)
        {
            
        }
    }

    public class Bucket
    {
        private BufferGraphic buffer;
        public Vector3 position;
        public List<Cell> cells;

        public bool IsVisible;

        public Bucket(BufferGraphic buffer)
        {
            this.buffer = buffer;
            this.cells = new List<Cell>();
            this.IsVisible = false;
            this.position = new Vector3();
        }

        public void SetVisibilty(bool isVisible)
        {
            this.IsVisible = isVisible;


            //if (isVisible)
            //{
            //    if (!cell._isVisible)
            //    {
            //        cell.AddComponent(new Graphic(buffer, cell,
            //            new ObjectTransform(new Vector2((int) cell.position.x, (int) cell.position.y)),
            //            cell.get_type()));
            //    }
            //}
            //else    
            //{

            //    if (component.GetType() == typeof(Graphic))
            //    {
            //        CellComponent component = cell.GetComponent<Graphic>();
            //        component.Clean();
            //        Cell.Destroy(component);
            //    }
            //}

            //cell.SetIsVisible(isVisible);
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
        //distance += (transform.position - newPosition).magnitude;
        transform.position = newPosition;

        if (IsOn)
        {
            if (buckets == null)
                return;
            if (hash == null)
            {
                RestartCamera(buffer);
                return;
            }
             Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);

            for (int x = 0; x < (int)Mathf.Sqrt(max_bucket); x++)
            {
                for (int y = 0; y < (int)Mathf.Sqrt(max_bucket); y++)
                {

                    Bucket bucket = this.buckets[x, y];
                    Vector2 x_Axis = new Vector2(bucket.position.x - positionCamera.x, 0);

                    if ((bucket.position - positionCamera).magnitude <= LengthVisibility &&
                        (bucket.position - positionCamera).magnitude >= -LengthVisibility)
                    {
                        if (!bucket.IsVisible)
                        {
                            bucket.SetVisibilty(true);

                            this.bucketsToLoad.Enqueue(this.buckets[x, y]);
                            StartCoroutine(Check());
                        }
                    }
                    else
                    {
                        if (bucket.IsVisible)
                        {
                            bucket.SetVisibilty(false);

                            this.bucketsToDeload.Enqueue(this.buckets[x, y]);
                         
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

        public Bucket[,] cutMap(BufferGraphic buffer,int max_bucket, Map map)
        {
            if (map == null)
                return null;
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
                                this.buckets[j, i] = new Bucket(buffer);
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



    void OnDrawGizmos()
    {
        if (this.buckets != null)
        {
            Vector3 positionCamera = this.transform.position + new Vector3(0, 0, 10);
            for (int x = 0; x < (int)(Mathf.Sqrt(max_bucket)); x++)
            {
                for (int y = 0; y < (int)(Mathf.Sqrt(max_bucket)); y++)
                {
                    float distX = (this.buckets[x, y].position.x - positionCamera.x) *2.0f;
                    float disty = this.buckets[x, y].position.y - positionCamera.y;
                    if ((this.buckets[x, y].position - positionCamera).magnitude <= LengthVisibility &&
                        (this.buckets[x, y].position - positionCamera).magnitude >= -LengthVisibility)
                    {
                        Gizmos.color = Color.green;
                    }
                    else
                    {
                        Gizmos.color = Color.white;
                    }

                    Gizmos.DrawSphere(this.buckets[x, y].position, 2.0f);
                }
            }

            Gizmos.color = Color.black;
            Gizmos.DrawSphere(positionCamera, 2.0f);
        }
    }

}
