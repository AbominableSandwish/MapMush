using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            gameManager.ChangeCellSelect(new Vector2Int(-1, 0));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gameManager.ChangeCellSelect(new Vector2Int(+1, 0));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            gameManager.ChangeCellSelect(new Vector2Int(0, +1));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameManager.ChangeCellSelect(new Vector2Int(0, -1));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.Generate();
        }
    }
}
