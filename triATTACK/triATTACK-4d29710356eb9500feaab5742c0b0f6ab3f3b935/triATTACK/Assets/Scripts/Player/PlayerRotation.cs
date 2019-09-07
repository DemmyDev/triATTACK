using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    Transform reticle;

    private void Start()
    {
        reticle = FindObjectOfType<Crosshair>().transform;
        reticle.GetComponent<Crosshair>().PlayerRef(transform);
    }

    void Update ()
    {
        if (!PauseMenu.isPaused)
        {
            Vector2 direction = new Vector2(reticle.position.x - transform.position.x, reticle.position.y - transform.position.y);
            transform.up = direction;
        }
    }
}
