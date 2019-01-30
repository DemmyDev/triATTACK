using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTeleport : MonoBehaviour {

    public Transform downSpawn;
    private Transform cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            other.transform.position = downSpawn.transform.position;

            Vector3 newCamPos = new Vector3(cam.transform.position.x, cam.transform.position.y - 10f, cam.transform.position.z);
            cam.transform.position = newCamPos;
        }
    }
}
