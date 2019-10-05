using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenOptions : MonoBehaviour
{
    [SerializeField] GameObject plainText;
    [SerializeField] Transform hitParticle;
    [SerializeField] GameObject optionsMenu;
    ScreenShake shake;

    GameObject playButton;

    [SerializeField] FlashSelect[] flashSelectors;
    [SerializeField] VolumeSelect[] volumeSelectors;
    [SerializeField] MusicSelect[] musicSelectors;
    PlayerShooting player;

    void Start()
    {
        playButton = transform.parent.gameObject;
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

            plainText.SetActive(false);
            optionsMenu.SetActive(true);
            foreach (FlashSelect flash in flashSelectors) flash.CheckOption();
            foreach (VolumeSelect volume in volumeSelectors) volume.CheckOption();
            foreach (MusicSelect music in musicSelectors) music.CheckOption();

            playButton.gameObject.SetActive(false);
        }
    }
}
