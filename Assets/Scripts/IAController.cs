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
                            cells = search.Research(position.x, position.y, 3);

                            if (cells.Count <= 2)
                            {
                                isBlocking = true;
                                return;
                            }

                            if (cells != null)
                            {
                                int index = Random.Range(0, cells.Count - 1);
                                targetPositon = new Vector2Int((int) cells[index].position.x,
                                    (int) cells[index].position.y);

                                path = search.Path(new Vector2Int(position.x, position.y), targetPositon);
                                if (path != null)
                                {
                                    nextPositon = convertTileCoordInScreenCoord((int)path[0].position.x, (int)path[0].position.y);
                                    direction = (nextPositon - new Vector2(transform.position.x, transform.position.y)).normalized;
                                    state = State.MOVE;
                                }
                            }


                        }
                    }
                }

                break;

            case State.MOVE:
                    transform.position += new Vector3(direction.x, direction.y) * Time.deltaTime;

                    if (transform.position.magnitude - nextPositon.magnitude <= .03f &&
                        nextPositon.magnitude - transform.position.magnitude <= .03f)
                    {
                        if (path.Count != 0)
                        {

                            transform.position = nextPositon;
                            position = new Vector2Int((int)path[0].position.x, (int)path[0].position.y);

                            var cell = path[0];


                            nextPositon = convertTileCoordInScreenCoord((int)cell.position.x, (int)cell.position.y);
                            //nextPositon = new Vector2(-5 + cell.position.x * 0.5f, -5 + cell.position.y * 0.5f);

                            direction = (nextPositon - new Vector2(transform.position.x, transform.position.y)).normalized;

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

                break;
        }

    }

    private void TurnFinish()
    {


    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f +((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f +((tileCoordX + tileCoordY) * (0.5f/2)));
        return screenCoord;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 0.5f);
    //    Gizmos.DrawSphere(nextPositon, 0.5f);
    //}
}
