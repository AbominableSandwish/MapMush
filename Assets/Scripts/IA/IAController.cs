using System.Collections;
using System.Collections.Generic;
using MapGame;
using UnityEngine;

public class IAController : MonoBehaviour
{
    enum State
    {
        IDLE,
        MOVE,
        ClIMB,
        BREAK
    }

    public bool isWaiting = true;
    private State state = State.IDLE;

    private Map map;
    private Cell cell;
    private SpriteRenderer render;
    //private UIMapManager Ui;

    public void Init(Map map)
    {
        search = new Search();
        this.map = map;
        this.render = GetComponentInChildren<SpriteRenderer>();


        GetComponentInChildren<Animator>().speed += Random.Range(-0.4f, 0.4f);
        //Ui = GameObject.Find("Map").GetComponent<UIMapManager>();

    }

    public void SetPosition(Vector2Int position)
    {
        this.position = position;

        cells = new List<Cell>();
        search = new Search();
        cells = search.Research(position.x, position.y, 1);

        if (render != null && cells[0] != null)
            render.sortingOrder = cells[0].sortingLayer + 2;
    }

    private Vector2Int position;
    public Vector2Int targetPositon;
    private List<Cell> cells;
    public List<Cell> path;
    public Search search;

    public Cell cellStart;
    public Cell celltarget;

    public Vector2 nextCellPosition;

    public float distance;

    private bool isBlocking = false;

    //Move
    [SerializeField] private Vector2 direction;
    public Vector2 startPosition;
    public Vector2 nextPositon;
    private int indexPath = 0;

    private float timeToWait = 1.5f;
    private float time;
    private float height;
    private float nextHeight;

    private int pathUI;

    public void CalcNewPosition(int indexPath)
    {
        if (path != null)
        {
            celltarget = path[indexPath + 1];
            GetComponentInChildren<Animator>().SetBool("IsMoving", true);

            startPosition = convertTileCoordInScreenCoord((int)path[indexPath].position.x, (int)path[indexPath].position.y);
            nextPositon = convertTileCoordInScreenCoord((int)path[indexPath + 1].position.x, (int)path[indexPath + 1].position.y);

            if (path[indexPath].position.z != path[indexPath + 1].position.z)
            {
                height = path[indexPath + 1].position.z - path[indexPath].position.z;

            }

            nextHeight = path[indexPath + 1].position.z;
            distance = (new Vector3(startPosition.x, startPosition.y) - new Vector3(nextPositon.x, nextPositon.y)).magnitude;
            direction = (new Vector3(nextPositon.x, nextPositon.y) - new Vector3(startPosition.x, startPosition.y)).normalized;

            // FLIP
            if (direction.x < 0)
            {
                render.flipX = true;
            }
            else
            {
                render.flipX = false;
            }
        }
    }

    // Update is called once per frame
    public void Move()
    {
        switch (state)
        {
            case State.IDLE:
                if (!isWaiting)
                {
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
                                    int indexPath = Random.Range(0, cells.Count - 1);
                                    targetPositon = new Vector2Int((int)cells[indexPath].position.x,
                                        (int)cells[indexPath].position.y);

                                    path = new List<Cell>();
                                    path = search.Path(new Vector2Int(position.x, position.y), targetPositon);

                                    if (path != null)
                                    {

                                        CalcNewPosition(this.indexPath);

                                        if (direction.x < 0)
                                        {
                                            render.sortingOrder = celltarget.sortingLayer + 2;
                                        }

                                        if (direction.y < 0)
                                        {
                                            render.sortingOrder = celltarget.sortingLayer + 2;
                                        }

                                        //   pathUI = Ui.AddPath(path);

                                        state = State.MOVE;

                                    }
                                }
                            }
                        }
                    }
                }
                break;

            case State.MOVE:
                if (this.indexPath != path.Count - 1)
                {
                    float dist = (startPosition - nextPositon).magnitude;


                    if (dist <= distance / 2)
                    {
                        if (height != 0.0f)
                        {
                            state = State.ClIMB;
                            return;
                        }

                        if (dist <= 0.1f)
                        {
                            render.sortingOrder = celltarget.sortingLayer + 2;
                            //POSITION
                            transform.position = new Vector3(nextPositon.x, nextPositon.y + nextHeight);

                            map.matrix[position.x, position.y].SetObject(null);

                            //TODO
                            path[indexPath].SetObject(this.gameObject);


                            position = new Vector2Int((int)path[indexPath].position.x, (int)path[indexPath].position.y);
                            indexPath++;

                            if (this.indexPath != path.Count - 1)
                            {
                                CalcNewPosition(indexPath);
                            }

                            if (direction.x < 0)
                            {
                                if (direction.y < 0)
                                    render.sortingOrder = celltarget.sortingLayer + 2;
                            }

                            if (direction.y < 0)
                            {
                                render.sortingOrder = celltarget.sortingLayer + 2;
                            }

                            GetComponentInChildren<Animator>().SetBool("IsMoving", true);
                        }
                    }
                }
                else
                {
                    if (startPosition.magnitude - nextPositon.magnitude <= .1f)
                    {
                        //move_object(path[0].position, path[i - 1].position);
                        position = new Vector2Int((int)path[indexPath].position.x, (int)path[indexPath].position.y);
                        transform.position = new Vector3(nextPositon.x, nextPositon.y + nextHeight);

                        direction = Vector2.zero;
                        nextPositon = Vector2.zero;
                        cells = null;

                        GetComponentInChildren<Animator>().SetBool("IsMoving", false);
                        isWaiting = true;

                        time = Time.time + timeToWait;

                        render.sortingOrder = celltarget.sortingLayer + 2;

                        celltarget = null;
                      //  Ui.RemovePath(pathUI);
                        this.indexPath = 0;

                        state = State.IDLE;
                        return;
                    }
                }

                transform.position += new Vector3(direction.x, direction.y) * Time.deltaTime;
                startPosition += direction * Time.deltaTime;
                break;

            case State.ClIMB:
                transform.position += new Vector3(0, height);

                render.sortingOrder = celltarget.sortingLayer + 2;

                height = 0.0f;
                state = State.MOVE;
                break;
        }

    }

    private void TurnFinish()
    {


    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 0.5f);
    //    Gizmos.DrawSphere(nextPositon, 0.5f);
    //}
}
