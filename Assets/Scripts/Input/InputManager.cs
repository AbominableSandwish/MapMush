using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class InputManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraManager cameraManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraManager = Camera.main.GetComponent<CameraManager>();
        Cursor.lockState = CursorLockMode.Confined;
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

        if (cameraManager.cameraMode == CameraManager.Mode.FREE)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                cameraManager.SetDirectionX(0);

            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                cameraManager.SetDirectionX(0);
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                cameraManager.SetDirectionY(0);
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                cameraManager.SetDirectionY(0);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                cameraManager.SetDirectionX(-1);
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
               cameraManager.SetDirectionX(+1);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraManager.SetDirectionY(+1);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraManager.SetDirectionY(-1);
            }

          
        }

       // if(Camera.main.WorldToScreenPoint(Input.mousePosition))
       //Cursor.visible
        //Debug.Log(Camera.main.(Input.mousePosition));
        //if(MouseOverEvent
        
    }
}
