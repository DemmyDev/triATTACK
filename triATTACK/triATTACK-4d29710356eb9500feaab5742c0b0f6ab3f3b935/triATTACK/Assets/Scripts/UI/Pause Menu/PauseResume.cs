using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseResume : MonoBehaviour
{
    Text text;
    PauseMenu pauseMenu;

    void Start()
    {
        text = GetComponent<Text>();
        pauseMenu = transform.parent.parent.GetComponent<PauseMenu>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Crosshair"))
        {
            text.color = new Color(1, 0, 0, 1);

            if (Input.GetButtonDown("Shoot"))
            {
                pauseMenu.Resume();
                text.color = new Color(1, 0, 0, 1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Crosshair"))
        {
            text.color = new Color(1, 0, 0, .5f);
        }
    }
}
