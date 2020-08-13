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

    private bool mouseIsLock = true;
    // Update is called once per frame
    void Update()
    {
        mouseIsLock = false;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameManager.Generate();
        }

        if (cameraManager.cameraMode == CameraManager.Mode.FREE)
        {
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                cameraManager.SetDirectionX(0);
                mouseIsLock = true;

            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                cameraManager.SetDirectionX(0);
                mouseIsLock = true;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                cameraManager.SetDirectionY(0);
                mouseIsLock = true;
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                cameraManager.SetDirectionY(0);
                mouseIsLock = true;
            }


            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //gameManager.ChangeCellSelect(new Vector2Int(-1, 0));
                cameraManager.SetDirectionX(-1);
                mouseIsLock = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                cameraManager.SetDirectionX(1);
                mouseIsLock = true;
                //gameManager.ChangeCellSelect(new Vector2Int(+1, 0));
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                cameraManager.SetDirectionY(1);
                mouseIsLock = true;
                //gameManager.ChangeCellSelect(new Vector2Int(0, +1));
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                cameraManager.SetDirectionY(-1);
                mouseIsLock = true;
                //gameManager.ChangeCellSelect(new Vector2Int(0, -1));
            }

        }


        if (!mouseIsLock)
        {
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
                    dirCamera.y = (mousePosition.y - sizeDisplay.y / 100 * (100 - PercentBoardScreen)) /
                                  (sizeDisplay.y / 100 * PercentBoardScreen);
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
            else
            {
                dirCamera = Vector2.zero;
                cameraManager.SetDirectionX(dirCamera.x);
                cameraManager.SetDirectionY(dirCamera.y);
            }
        }
        Vector2 position = convertScreenCoordInCoord(cameraManager.transform.position);

        //GameObject.Find("Map").GetComponent<MapManager>().SelectCell(position, Color.blue);
       // Debug.Log(cameraManager.transform.position- Camera.main.ScreenToWorldPoint(Input.mousePosition));

        //GameObject.Find("Map").GetComponent<MapManager>().SelectCell(convertScreenCoordInCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Color.green);

        //if (Input.GetMouseButtonDown(0))
        //{
        //    gameManager.Select(convertScreenCoordInCoord(Camera.main.ScreenToWorldPoint(Input.mousePosition))) ;
        //}

    }


    private Vector2 convertScreenCoordInCoord(Vector3 screenCoord)
    {
        Vector2 coord;
        coord.x = (int)((screenCoord.y + screenCoord.x*0.5f))-0.5f;
        coord.y = (int)((screenCoord.y - screenCoord.x*0.5f))-0.5f;
        return coord;
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX + tileCoordY)));
        screenCoord.y = (float)(-4.80f + ((tileCoordX - tileCoordY) * (0.5f)));
        return screenCoord;
    }

}
