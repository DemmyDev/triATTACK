using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsToMenu : MonoBehaviour
{
    [SerializeField] GameObject plainText;
    [SerializeField] Transform hitParticle;
    [SerializeField] GameObject optionsMenu;
    ScreenShake shake;

    [SerializeField] GameObject playButton;
    PlayerShooting player;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
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

            plainText.SetActive(true);
            playButton.gameObject.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }
}
