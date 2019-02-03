using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTeleport : MonoBehaviour {

    public Transform rightSpawn;
    private Transform cam;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = rightSpawn.transform.position;

            Vector3 newCamPos = new Vector3(cam.transform.position.x + 16f, cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = newCamPos;
        }
    }
}
