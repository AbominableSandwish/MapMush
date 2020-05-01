using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    enum State
    {
        IDLE,
        MOVE,
        WAIT
    }

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

    //Move
    private Vector2 direction;
    public Vector2 nextPositon;

    private float timeToWait = 1.5f;
    private float time;


    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                if (Time.time >= time)
                {
                    if (position != null)
                    {
                        cells = search.Research(position.x, position.y, 50);
                        if (cells != null)
                        {
                            targetPositon = new Vector2Int(Random.Range(0, 19), Random.Range(0, 19));


                            path = search.Path(new Vector2Int(position.x, position.y), targetPositon);
                            if (path != null)
                            {
                                path.Remove(path[0]);
                                path.Remove(path[0]);
                                Debug.Log("PlayerPosition : " + transform.position);
                                Debug.Log("NextPosition : " + new Vector2(-5 + path[0].position.x * 0.5f,
                                              -5 + path[0].position.y * 0.5f));
                                state = State.MOVE;
                            }
                        }

                    }
                }

                break;

            case State.MOVE:
                if (direction != Vector2.zero)
                {
                    transform.position += new Vector3(direction.x, direction.y)* Time.deltaTime;

                    if (transform.position.magnitude - nextPositon.magnitude <= .01f &&
                        nextPositon.magnitude - transform.position.magnitude <= .01f)
                    {
                        transform.position = nextPositon;
                        this.direction = Vector2.zero;
                        position = new Vector2Int((int)path[0].position.x, (int)path[0].position.y);
                        path.Remove(path[0]);
                        if (path.Count == 0)
                        {
                            //move_object(path[0].position, path[i - 1].position);

                            transform.position = nextPositon;

                            direction = Vector2.zero;

                            nextPositon = Vector2.zero;

                            path = null;

                            GetComponentInChildren<Animator>().SetBool("IsMoving", false);

                            time = Time.time + timeToWait;

                            state = State.IDLE;
                        }

                    }
                }
                else
                {
                    if (path.Count >= 0)
                    {
                        var cell = path[0];
                        nextPositon= convertTileCoordInScreenCoord((int)cell.position.x, (int)cell.position.y);
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
                    }

                }
                break;

            case State.WAIT:
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
