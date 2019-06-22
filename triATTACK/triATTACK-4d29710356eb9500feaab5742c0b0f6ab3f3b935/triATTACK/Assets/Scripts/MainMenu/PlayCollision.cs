using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCollision : MonoBehaviour
{
    [SerializeField] Transform hitParticle;
    [SerializeField] Transform selectUI;
    Text playText;
    MainMenu mainMenu;
    BoxCollider2D col;
    ScreenShake shake;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        col = GetComponent<BoxCollider2D>();
        playText = GetComponent<Text>();
        mainMenu = transform.parent.GetComponent<MainMenu>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            playText.enabled = false;
            col.enabled = false;
            mainMenu.StartGame();
        }
    }
}
