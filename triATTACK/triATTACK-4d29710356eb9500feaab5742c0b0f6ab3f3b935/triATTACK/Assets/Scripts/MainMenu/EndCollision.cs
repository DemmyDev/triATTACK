using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCollision : MonoBehaviour 
{
    Text playText;
    MainMenu mainMenu;
    BoxCollider2D col;

    void Start()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
        playText = gameObject.GetComponent<Text>();
        mainMenu = gameObject.transform.parent.GetComponent<MainMenu>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            FindObjectOfType<AudioManager>().Play("TextEnter");
            playText.CrossFadeAlpha(2f, 0f, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            col.enabled = false;
            FindObjectOfType<AudioManager>().Play("TextExit");
            playText.CrossFadeAlpha(1f, 0f, true);
            mainMenu.QuitGame();
        }
    }
}
