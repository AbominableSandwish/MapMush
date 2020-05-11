using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    enum State
    {
        IDLE,
        MOVE
    }

    public bool isWaiting = true;
    private State state = State.IDLE;

    private Map map;
    private Cell cell;


    // Start is called before the first frame update
    void Start()
    {
        search = new Search();
        map = GameObject.Find("Map").GetComponent<MapManager>().GetMap();
    }

    public void SetPosition(Vector2Int position)
    {
        this.position = position;

    }

    private Vector2Int position;
    public Vector2Int targetPositon;
    private List<Cell> cells;
    public List<Cell> path;
    public Search search;

    public Cell cellStart;
    private Cell cellTarget;

    public Vector2 nextCellPosition;

    public float distance;

    private bool isBlocking = false;

    //Move
    private Vector2 direction;
    public Vector2 nextPositon;

    private float timeToWait = 1.5f;
    private float time;


    // Update is called once per frame
    public void Move()
    {
        switch (state)
        {
            case State.IDLE:
                if (!isBlocking)
                {
                    this.direction = Vector2.zero;
                    nextPositon = Vector2.zero;
                    if (Time.time >= time)
                    {
                        if (position != null)
                        {

                            cells = new List<Cell>();
                            search = new Search();
                            cells = search.Research(position.x, position.y, 3);

                            if (cells.Count <= 2)
                            {
                                isBlocking = true;
                                return;
                            }

                            if (cells != null)
                            {
                                int index = Random.Range(0, cells.Count - 1);
                                targetPositon = new Vector2Int((int)cells[index].position.x,
                                    (int)cells[index].position.y);

                                path = new List<Cell>();
                                path = search.Path(new Vector2Int(position.x, position.y), targetPositon);
                                if (path != null)
                                {
                                    nextPositon = convertTileCoordInScreenCoord((int)path[0].position.x, (int)path[0].position.y);
                                    nextPositon += new Vector2(0.0f, path[0].position.z);
                                    distance = (transform.position - new Vector3(nextPositon.x, nextPositon.y))
                                        .magnitude;
                                    direction = (nextPositon - new Vector2(transform.position.x, transform.position.y)).normalized;
                                    
                                        GetComponentInChildren<SpriteRenderer>().sortingOrder =
                                            ((map.GetHeight() - (int)position.y) + (map.GetWidth() - (int)position.x)) * 2 + 1;
                                    state = State.MOVE;
                                }
                            }
                        }
                    }
                }
                break;

            case State.MOVE:
                transform.position += new Vector3(direction.x, direction.y) * Time.deltaTime;
                SpriteRenderer render = GetComponentInChildren<SpriteRenderer>();

                if (transform.position.magnitude - nextPositon.magnitude <= .01f &&
                    nextPositon.magnitude - transform.position.magnitude <= .01f)
                {

                    

                    render.sortingOrder =
                        ((map.GetHeight() - (int)position.y) +
                         (map.GetWidth() - (int)position.x)) * 2 + 1;


                    if (path.Count != 0)
                    {


                        if (direction.x > 0)
                        {
                            render.sortingOrder =
                                ((map.GetHeight() - (int)position.y) +
                                 (map.GetWidth() - (int)position.x)) * 2 + 1;
                        }

                        if (direction.y > 0)
                        {
                            render.sortingOrder =
                                ((map.GetHeight() - (int)position.y) +
                                 (map.GetWidth() - (int)position.x)) * 2 + 1;
                        }

                        transform.position = nextPositon;
                        map.matrix[position.x, position.y].SetObject(null);
                        position = new Vector2Int((int)path[0].position.x, (int)path[0].position.y);
                        path[0].SetObject(this.gameObject);

                       

                        var cell = path[0];

                        nextPositon = convertTileCoordInScreenCoord((int)cell.position.x, (int)cell.position.y);
                        nextPositon += new Vector2(0, cell.position.z);
                        //nextPositon = new Vector2(-5 + cell.position.x * 0.5f, -5 + cell.position.y * 0.5f);

                        distance = (transform.position - new Vector3(nextPositon.x, nextPositon.y))
                            .magnitude;
                        direction = (nextPositon - new Vector2(transform.position.x, transform.position.y)).normalized;

                        if (direction.x > 0)
                        {
                            render.sortingOrder =
                                ((map.GetHeight() - (int)position.y) +
                                 (map.GetWidth() - (int)position.x)) * 2 + 1;
                        }

                        //if (direction.y < 0)
                        //{
                        //    render.sortingOrder =
                        //        ((map.GetHeight() - (int)position.y) +
                        //         (map.GetWidth() - (int)position.x)) * 2 + 1;
                        //}


                        if (direction.x < 0)
                        {
                            GetComponentInChildren<SpriteRenderer>().flipX = true;
                        }
                        else
                        {
                            GetComponentInChildren<SpriteRenderer>().flipX = false;
                        }

                        GetComponentInChildren<Animator>().SetBool("IsMoving", true);
                        path.Remove(path[0]);
                    }
                    else
                    {
                        //move_object(path[0].position, path[i - 1].position);

                        transform.position = nextPositon;

                        direction = Vector2.zero;

                        nextPositon = Vector2.zero;

                        cells = null;

                        GetComponentInChildren<Animator>().SetBool("IsMoving", false);

                        time = Time.time + timeToWait;

                        isWaiting = true;

                        state = State.IDLE;
                        return;
                    }
                }
                else
                {
                    if((transform.position - new Vector3(nextPositon.x, nextPositon.y)).magnitude <= distance / 4)
                    {
                        if (direction.x < 0)
                        {
                            render.sortingOrder =
                                ((map.GetHeight() - (int)position.y) +
                                 (map.GetWidth() - (int)position.x)) * 2 + 1;
                        }
                        if (direction.y < 0)
                        {
                            render.sortingOrder =
                                ((map.GetHeight() - (int)position.y) +
                                 (map.GetWidth() - (int)position.x)) * 2 + 1;
                        }
                    }
                }

                break;
        }

    }

    private void TurnFinish()
    {


    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 0.5f);
    //    Gizmos.DrawSphere(nextPositon, 0.5f);
    //}
}
