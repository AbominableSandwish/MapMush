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
    private Vector2 sizeDisplay;


    public float PercentBoardScreen;

    private Vector2 dirCamera;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraManager = Camera.main.GetComponent<CameraManager>();
        Cursor.lockState = CursorLockMode.Confined;

        sizeDisplay = new Vector2(Display.main.renderingWidth, Display.main.renderingHeight);
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

        Vector2 mousePosition = Input.mousePosition;
        dirCamera = Vector2.zero;

        if ((mousePosition.x >= 0 && mousePosition.x <= sizeDisplay.x)
            && (mousePosition.y >= 0 && mousePosition.y <= sizeDisplay.y))
        {
            Debug.Log("IN");
            if (mousePosition.y < sizeDisplay.y / 100 * PercentBoardScreen)
            {
                dirCamera.y = -(1 - (mousePosition / (sizeDisplay / 100 * PercentBoardScreen)).y);
            }


            if (mousePosition.y > sizeDisplay.y / 100 * (100 - PercentBoardScreen))
            {
                dirCamera.y = (mousePosition.y - sizeDisplay.y / 100 * (100 - PercentBoardScreen)) / (sizeDisplay.y / 100 * PercentBoardScreen);
            }


            if (mousePosition.x < sizeDisplay.x / 100 * 15)
            {
                if (mousePosition.x < sizeDisplay.x / 100 * 5)
                {
                    dirCamera.x = -1f;
                }
                else
                {
                    dirCamera.x = -0.5f;
                }
            }

            if (mousePosition.x > sizeDisplay.x / 100 * 85)
            {
                if (mousePosition.x > sizeDisplay.x / 100 * 95)
                {
                    dirCamera.x = 1f;
                }
                else
                {
                    dirCamera.x = 0.5f;
                }
            }


            cameraManager.SetDirectionX(dirCamera.x);
            cameraManager.SetDirectionY(dirCamera.y);
        }
    }

}
