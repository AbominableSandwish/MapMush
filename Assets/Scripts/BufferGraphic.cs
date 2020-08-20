using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class BufferGraphic
{
    private List<Transform> tansforms;
    private MapManager mapManager;

    public int nbrMaxTransform = 200000;

    private Material material;

    class RenderGraphic
    {
        private int id;
        private Transform tranform;

        public RenderGraphic(Transform transform, int id)
        {
            this.tranform = transform;
            this.id = id;
        }

        public Transform GetTransform()
        {
            return this.tranform;
        }

        public void SetTransform(Transform transform)
        {
            this.tranform = transform;
        }

    }

     public enum TypeTexture
    {
        Ground,
        Tree,
        Plant
    }

    public List<Transform> bufferGrounds;
    public List<Transform> bufferTrees;

    public Queue<Transform> gameObjectsOnHold;


    public List<Transform> gameObjectsUsed;

    private Transform transformParentBuffer;
    private Transform transformParentMap;

    // Start is called before the first frame update
    public BufferGraphic(MapManager mapManager)
    {
        this.mapManager = mapManager;
        gameObjectsOnHold = new Queue<Transform>();
        transformParentBuffer = GameObject.Find("MapBuffer").transform;
        transformParentMap = GameObject.Find("Map").transform;

        //Buffer
        bufferGrounds = new List<Transform>();
        bufferTrees = new List<Transform>();

        material = mapManager.material;
      
        GameObject objW;
        string nameW = "prefab_Water";
        objW = GameObject.Instantiate(new GameObject(nameW).transform, transformParentBuffer).gameObject;
        objW.transform.localPosition = new Vector3(0, 1);
        objW.AddComponent<SpriteRenderer>();
        objW.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Water;
        objW.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        objW.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
        objW.GetComponent<SpriteRenderer>().material = material;
        bufferGrounds.Add(objW.transform);

        {
            GameObject obj;
            string name = "prefab_Dirt";
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.transform.localPosition = new Vector3(0, 4);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Dirt;
            obj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
            obj.GetComponent<SpriteRenderer>().size = new Vector2(2, 1.75f);
            obj.GetComponent<SpriteRenderer>().material = material;
            bufferGrounds.Add(obj.transform);
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
            obj.GetComponent<SpriteRenderer>().material = material;
            bufferGrounds.Add(obj.transform);
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
            obj.GetComponent<SpriteRenderer>().material = material;
            bufferGrounds.Add(obj.transform);
        }

        {
            GameObject obj;
            string name = "prefab_Tree";
            obj = GameObject.Instantiate(new GameObject(name).transform, transformParentBuffer).gameObject;
            obj.transform.localPosition = new Vector3(2, 0);
            obj.AddComponent<SpriteRenderer>();
            obj.GetComponent<SpriteRenderer>().sprite = this.mapManager.tile_Tree;
            obj.GetComponent<SpriteRenderer>().material = material;
            bufferTrees.Add(obj.transform);
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
            obj.GetComponent<SpriteRenderer>().material = material;
            gameObjectsOnHold.Enqueue(obj.transform);
        }

        



        Debug.Log("Load Graphics");


    }

    private const int IsometricRangePerYUnit = 10;

    public Transform AddGameObject(Vector3 position, TypeTexture type, int texture, int sortingLayer)
    {
        Transform transform = gameObjectsOnHold.Dequeue();

        transform.parent = transformParentMap;
        transform.localPosition = position;
        SpriteRenderer render = transform.gameObject.GetComponent<SpriteRenderer>();


        int layer = sortingLayer;
        switch (type)
        {
            case TypeTexture.Ground:
                if (texture == 0)
                {
                    render.sprite = null;
                }
                if (texture == 1)
                {
                    render.sprite = bufferGrounds[1].gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                if (texture == 2)
                {
                    render.sprite = bufferGrounds[2].gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                if (texture == 3)
                {
                    render.sprite = bufferGrounds[3].gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                if (texture == 4)
                {
                    render.sprite = bufferGrounds[4].gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                if (texture == 5)
                {
                    render.sprite = bufferGrounds[5].gameObject.GetComponent<SpriteRenderer>().sprite;
                }
                
                render.drawMode = SpriteDrawMode.Sliced;
                render.size = new Vector2(2, 1.75f);
                break;

            case TypeTexture.Tree:
                if (texture == 0)
                {
                    layer += 5;
                    render.sprite = bufferTrees[0].gameObject.GetComponent<SpriteRenderer>().sprite;
                    render.drawMode = SpriteDrawMode.Sliced;
                    render.size = new Vector2(4, 4);
                }
                break;
        }

        render.sortingOrder = layer;//-(int)(transform.position.y * IsometricRangePerYUnit);
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
        gameObjectsOnHold.Enqueue(transform);
    }
}
