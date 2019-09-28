using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndCollision : MonoBehaviour 
{
    Text playText;
    MainMenu mainMenu;
    BoxCollider2D col;
    ScreenShake shake;
    [SerializeField] GameObject hitParticle;


    void Start()
    {
        col = gameObject.GetComponent<BoxCollider2D>();
        playText = gameObject.GetComponent<Text>();
        mainMenu = gameObject.transform.parent.parent.GetComponent<MainMenu>();
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            AudioManager.Instance.Play("TextEnter");
            col.enabled = false;
            playText.enabled = false;
            mainMenu.QuitGame();
        }
    }
}
