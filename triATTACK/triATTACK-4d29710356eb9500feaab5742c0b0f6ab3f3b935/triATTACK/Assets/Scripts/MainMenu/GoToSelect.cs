using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToSelect : MonoBehaviour
{
    [SerializeField] GameObject indicatorText;
    [SerializeField] GameObject plainText;
    [SerializeField] GameObject selectUI;
    [SerializeField] Transform hitParticle;
    Text playText;
    BoxCollider2D col;
    ScreenShake shake;

    MainMenu mainMenu;

    [SerializeField] bool playtesting;
    PlayerShooting player;

    void Start()
    {
        mainMenu = transform.parent.GetComponent<MainMenu>();
        shake = Camera.main.GetComponent<ScreenShake>();
        col = GetComponent<BoxCollider2D>();
        playText = GetComponent<Text>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            player.ForceRecall();
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            playText.enabled = false;
            col.enabled = false;
            indicatorText.SetActive(false);

            if (playtesting) mainMenu.StartGame();
            else
            {
                plainText.SetActive(false);
                selectUI.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
