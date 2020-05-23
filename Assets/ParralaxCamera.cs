using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxCamera : MonoBehaviour
{
    [SerializeField] private List<GameObject> cloudsPrefab;

    const int CLOUD_MAX = 90;
    private List<Transform> clouds;



    // Start is called before the first frame update
    void Start()
    {
        clouds = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clouds.Count != CLOUD_MAX)
        {
            var cloud = Instantiate(cloudsPrefab[Random.Range(0, cloudsPrefab.Count - 1)], transform);
            clouds.Add(cloud.transform);
            cloud.transform.localPosition = new Vector3(Random.Range(-20.0f, 20.0f), Random.Range(-15.0f, 15.0f), 10);
        }
    }
}
