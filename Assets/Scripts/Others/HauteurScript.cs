using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauteurScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isFlying)
        {
            counter += Time.deltaTime;
            transform.position += new Vector3(0, Mathf.Sin(counter*2) / 200);
        }
    }

    private bool isFlying = false;
    public void SetFly()
    {
        transform.position -= new Vector3(0, Random.Range(-0.4f,0.4f));
        isFlying = true;
        counter = Random.Range(0, 1000);
    }

    private Vector2 convertTileCoordInScreenCoord(int tileCoordX, int tileCoordY)
    {
        Vector2 screenCoord;
        screenCoord.x = (float)(-0.25f + ((tileCoordX - tileCoordY) * 0.5f));
        screenCoord.y = (float)(-4.80f + ((tileCoordX + tileCoordY) * (0.5f / 2)));
        return screenCoord;
    }



    private float counter = 0.0f;

}
