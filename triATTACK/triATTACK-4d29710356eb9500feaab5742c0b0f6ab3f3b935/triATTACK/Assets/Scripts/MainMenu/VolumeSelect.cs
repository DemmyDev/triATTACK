using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSelect : MonoBehaviour
{
    [SerializeField] float volumeSet;
    [SerializeField] GameObject[] otherSelectors;
    [SerializeField] Transform hitParticle;

    BoxCollider2D col;
    Text text;
    ScreenShake shake;

    void Start ()
    {
        col = GetComponent<BoxCollider2D>();
        text = GetComponent<Text>();
        shake = Camera.main.GetComponent<ScreenShake>();

        if (volumeSet == AudioListener.volume)
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
            AudioListener.volume = volumeSet;
            col.enabled = false;
            text.CrossFadeAlpha(.25f, 0f, true);

            foreach (GameObject selector in otherSelectors)
            {
                selector.GetComponent<BoxCollider2D>().enabled = true;
                selector.GetComponent<Text>().CrossFadeAlpha(1f, 0f, true);
            }

            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("EnemyHit");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
        }
    }
}
