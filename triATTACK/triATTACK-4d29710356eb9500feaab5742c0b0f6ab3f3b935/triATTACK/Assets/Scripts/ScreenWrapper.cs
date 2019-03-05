using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrapper : MonoBehaviour
{
    float screenX = 36.25f, screenY = 20.75f;

    void Update()
    {
        Vector2 pos = transform.position;

        if (pos.x > screenX)
        {
            transform.position = new Vector2(-screenX, pos.y);
        }

        if (pos.x < -screenX)
        {
            transform.position = new Vector2(screenX, pos.y);
        }

        if (pos.y > screenY)
        {
            transform.position = new Vector2(pos.x, -screenY);
        }

        if (pos.y < -screenY)
        {
            transform.position = new Vector2(pos.x, screenY);
        }
    }
}