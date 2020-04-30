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

    private State state = State.IDLE;

    public BFS bfs;
    private Map map;

    // Start is called before the first frame update
    void Start()
    {
        bfs = new BFS();
        map = GameObject.Find("Map").GetComponent<MapManager>().GetMap();

        bfs.Research(Random.Range(0, 20), Random.Range(0, 20), 20, map);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.IDLE:
                break;

            case State.MOVE:
                break;
        }

    }

    private void TurnFinish()
    {

    }


}
