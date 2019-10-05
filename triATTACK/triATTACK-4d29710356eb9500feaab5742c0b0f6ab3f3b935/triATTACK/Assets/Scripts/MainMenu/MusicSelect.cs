using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSelect : MonoBehaviour
{
    [SerializeField] bool toggleMusic;
    [SerializeField] Transform hitParticle;
    [SerializeField] GameObject otherSelector;
    BoxCollider2D col;
    Text text;
    ScreenShake shake;
    PlayerShooting player;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        text = GetComponent<Text>();
        shake = Camera.main.GetComponent<ScreenShake>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>();
    }

    public void CheckOption()
    {
        col = GetComponent<BoxCollider2D>();
        text = GetComponent<Text>();

        if (toggleMusic == ReadWriteSaveManager.Instance.GetData("MusicOn", true, false))
        {
            col.enabled = false;
            text.CrossFadeAlpha(.25f, 0f, true);
        }
        else
        {
            col.enabled = true;
            text.CrossFadeAlpha(1f, 0f, true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            player.ForceRecall();
            ReadWriteSaveManager.Instance.SetData("MusicOn", toggleMusic, true);
            col.enabled = false;
            text.CrossFadeAlpha(.25f, 0f, true);

            otherSelector.GetComponent<BoxCollider2D>().enabled = true;
            otherSelector.GetComponent<Text>().CrossFadeAlpha(1f, 0f, true);

            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("EnemyHit");
            Instantiate(hitParticle, transform.position, Quaternion.identity);

            if (toggleMusic) AudioManager.Instance.Play("MusicMenu");
            else AudioManager.Instance.Stop("MusicMenu");
        }
    }
}
