using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour {

    private float timeBtwSpawns;
    public float startTimeBtwSpawns;

    public GameObject echoPrefab;

    void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            
            GameObject instance = (GameObject)Instantiate(echoPrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(instance, 1f);
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
