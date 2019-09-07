using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToSelect : MonoBehaviour
{
    [SerializeField] GameObject plainText;
    [SerializeField] GameObject selectUI;
    [SerializeField] Transform hitParticle;
    Text playText;
    BoxCollider2D col;
    ScreenShake shake;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        col = GetComponent<BoxCollider2D>();
        playText = GetComponent<Text>();
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

            plainText.SetActive(false);
            selectUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
