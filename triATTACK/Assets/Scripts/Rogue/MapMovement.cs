﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour {

    public Transform leftSpawn;
    public Transform cam;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            other.transform.position = leftSpawn.transform.position;

            Vector3 newCamPos = new Vector3(cam.transform.position.x - 17f, cam.transform.position.y, cam.transform.position.z);
            cam.transform.position = newCamPos;
        }
    }

}
