using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    [SerializeField] private int maxInAction = 100;
    private List<IAController> listIA;
    public bool isReady = false;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Player2;


    [SerializeField] private int nbrOfPlayer = 20;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (listIA != null)
        {
            if (listIA.Count != 0)
            {
                for (int i = 0; i < maxInAction; i++)
                {
                    if (!listIA[i].isWaiting)
                    {
                        listIA[i].Move();
                    }
                    else
                    {
                        IAController ia = listIA[i];
                        listIA.Remove(listIA[i]);
                        listIA.Add(ia);
                        ia.isWaiting = false;
                    }

                }
            }
        }
    }

    public void AddIA(IAController ia)
    {
        if(this.listIA == null)
            listIA = new List<IAController>();
        this.listIA.Add(ia);
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f / 2)));
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

            if (mapManager.GetMap().matrix[i, j].get_type() != 1)
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
                player.GetComponent<IAController>().SetPosition(new Vector2Int(i, j));
                player.transform.position = convertTileCoordInScreenCoord(i, j);
                //player.transform.position = new Vector3(-5 + 0.5f * i, -5 + 0.5f * j);
                mapManager.GetMap().AddObject(i, j, player);
                AddIA(player.GetComponent<IAController>());
                counter++;
            }
        }
    }
}
