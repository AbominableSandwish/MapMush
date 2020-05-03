using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAManager : MonoBehaviour
{
    [SerializeField] private int maxInAction = 100;
    private List<IAController> listIA;
    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
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

    public void AddIA(IAController ia)
    {
        if(this.listIA == null)
            listIA = new List<IAController>();
        this.listIA.Add(ia);
    }
}
