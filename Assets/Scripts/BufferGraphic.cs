using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BufferGraphic
{
    private List<Transform> tansforms;
    private MapManager mapManager;

    public int nbrMaxTransform = 20000;

    public List<Transform> bufferTransforms;

    public List<Transform> gameObjectsOnHold;

    public List<Transform> gameObjectsUsed;

    private Transform transformParentBuffer;
    private Transform transformParentMap;

    // Start is called before the first frame update
    public BufferGraphic(MapManager mapManager)
    {
        this.mapManager = mapManager;
        gameObjectsOnHold = new List<Transform>();
        transformParentBuffer = GameObject.Find("MapBuffer").transform;
        transformParentMap = GameObject.Find("Map").transform;

        //Buffer
        bufferTransforms = new List<Transform>();
      
        GameObject objW;
        string nameW = "prefab_Water";
        objW = GameObject.Instantiate(new GameObject(nameW).transform, transformParentBuffer).gameObject;
        objW.transform.localPosition = new Vector3(0, 1);
        objW.AddComponent<SpriteRenderer>();
        objW.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Water;
        objW.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        objW.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
        bufferTransforms.Add(objW.transform);

        {
            GameObject obj;
            string name = "prefab_Dirt";
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.transform.localPosition = new Vector3(0, 4);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Dirt;
            obj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
            bufferTransforms.Add(obj.transform);
        }
        {
            GameObject obj;
            string name = "prefab_Rock";
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.AddComponent<SpriteRenderer>();
            obj.transform.localPosition = new Vector3(0, 6);
            obj.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Rock;
            obj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
            bufferTransforms.Add(obj.transform);
        }
        {
            GameObject obj;
            string name = "prefab_HightRock";
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.transform.localPosition = new Vector3(0, 8);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_HightRock;
            obj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
            bufferTransforms.Add(obj.transform);
        }

        this.mapManager = mapManager;

        for (int i = 0; i < nbrMaxTransform; i++)
        {
            GameObject obj;
            string name = "Cell_" + (gameObjectsOnHold.Count+1).ToString();
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.transform.localPosition = new Vector3(gameObjectsOnHold.Count+1/2, 0.0f);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
            gameObjectsOnHold.Add(obj.transform);
        }

        



        Debug.Log("Load Graphics");


    }

    private const int IsometricRangePerYUnit = 10;

    public Transform AddGameObject(Vector3 position, int type)
    {
        Transform transform = gameObjectsOnHold[0];
        gameObjectsOnHold.Remove(gameObjectsOnHold[0]);


        transform.parent = transformParentMap;
        transform.localPosition = position;
        SpriteRenderer render = transform.gameObject.GetComponent<SpriteRenderer>();
        if (type == 0)
        {
            render.sprite = bufferTransforms[0].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        if (type == 1)
        {
            render.sprite = bufferTransforms[1].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        if (type == 2)
        {
            render.sprite = bufferTransforms[2].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        if (type == 3)
        {
            render.sprite = bufferTransforms[3].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        if (type == 4)
        {
            render.sprite = bufferTransforms[4].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        if (type == 5)
        {
            render.sprite = bufferTransforms[5].gameObject.GetComponent<SpriteRenderer>().sprite;
        }
        render.drawMode = SpriteDrawMode.Sliced;
        render.size= new Vector2(2, 1.75f);

        

        render.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
        if (gameObjectsUsed == null)
        {
            gameObjectsUsed = new List<Transform>();
        }
        gameObjectsUsed.Add(transform);
       

        return transform;
    }

    public void RemoveObjectGraphic(Transform transform)
    {
        transform.localPosition = Vector3.zero;

        gameObjectsUsed.Remove(transform);
        gameObjectsOnHold.Add(transform);
    }
}
