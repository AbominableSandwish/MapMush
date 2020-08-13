using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    [SerializeField] public int maxInAction = 100;
    private List<IAController> listAllIA;
    private IAController[] listIAInAction;
    public bool isReady = false;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Player2;

    [SerializeField] private int nbrOfPlayer = 20;
    private int counter;

    int GetActionFree()
    {
        int index = 0;
        foreach (var iaAction in this.listIAInAction)
        {
            if (iaAction == null)
            {
                break;
            }

            index++;
        }

        if (index == this.listIAInAction.Length)
        {
            index = -1;
        }

        return index;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (listAllIA != null)
        {
            if (counter < maxInAction)
            {
                int indexAction = GetActionFree();
                if (indexAction != -1)
                {

                    this.listIAInAction[indexAction] = listAllIA[0];
                    this.listIAInAction[indexAction].isWaiting = false;
                    listAllIA.Remove(listAllIA[0]);
                    listAllIA.Add(this.listIAInAction[indexAction]);
                    counter++;
                }
            }

            for (int i = 0; i < listIAInAction.Length - 1; i++)
            {
                if (listIAInAction[i] != null)
                {
                    listIAInAction[i].Move();
                    if (listIAInAction[i].isWaiting)
                    {
                        listIAInAction[i] = null;
                        counter--;
                    }
                }
            }







        }
        //if (listAllIA != null)
        //{
        //    if (listAllIA.Count != 0)
        //    {
        //        for (int i = 0; i < maxInAction; i++)
        //        {
        //            if (!listAllIA[i].isWaiting)
        //            {
        //                listAllIA[i].Move();
        //            }
        //            else
        //            {
        //                IAController ia = listAllIA[i];
        //                listIA.Remove(listIA[i]);
        //                listIA.Add(ia);
        //                ia.isWaiting = false;
        //            }

        //        }
        //    }
        //}
    }

    //public void IaFinished(IAController)
    //{
    //    listIA.Find()
    //    IAController ia = listIA[i];
    //    listIA.Remove(listIA[i]);
    //    this.counter--;
    //}

    public void AddIA(IAController ia)
    {

        if (this.listAllIA == null)
        {
            listAllIA = new List<IAController>();
        }

        if (maxInAction != 0)
        {
            this.listIAInAction = new IAController[maxInAction];
        }

        if (this.listAllIA.Count != 0)
            this.listAllIA.Insert(Random.Range(0, listAllIA.Count - 1), ia);
        else
            this.listAllIA.Add(ia);
    }

    private Vector3 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector3 screenCoord = new Vector3();
        screenCoord.x = (float)(((tileCoordX - tileCoordY)));
        screenCoord.y = (float)(((tileCoordX + tileCoordY) * (0.5f)));
        return screenCoord;
    }

    public void InstancePlayer(MapManager mapManager)
    {

        int counter = 0;
        while (counter < nbrOfPlayer)
        {

            int i, j;
            if (counter < nbrOfPlayer / 2)
            {
                i = Random.Range(0, mapManager.GetMap().GetWidth() / 2);
                j = Random.Range(0, mapManager.GetMap().GetHeight());
            }
            else
            {
                i = Random.Range(0, mapManager.GetMap().GetWidth() / 2) + mapManager.GetMap().GetWidth() / 2;
                j = Random.Range(0, mapManager.GetMap().GetHeight());
            }

            if (mapManager.GetMap().matrix[i, j].get_type() != 0)
            {
                if (mapManager.GetMap().matrix[i, j].Object == null)
                {
                    GameObject player;
                    if (counter < nbrOfPlayer / 2)
                    {
                        player = GameObject.Instantiate(Player, GameObject.Find("UnityMap").transform);
                    }
                    else
                    {
                        player = GameObject.Instantiate(Player2, GameObject.Find("UnityMap").transform);
                    }
                    player.GetComponent<IAController>().Init(mapManager.GetMap());
                    player.GetComponent<IAController>().SetPosition(new Vector2Int(i, j));
                    Vector3 position = convertTileCoordInScreenCoord(i, j);
                    position += new Vector3(0, mapManager.GetMap().matrix[i, j].position.z);
                    player.transform.position = position;

                    //player.transform.position = new Vector3(-5 + 0.5f * i, -5 + 0.5f * j);

                    mapManager.GetMap().AddObject(i, j, player);
                    AddIA(player.GetComponent<IAController>());

                    counter++;
                }
            }
        }
    }
}
