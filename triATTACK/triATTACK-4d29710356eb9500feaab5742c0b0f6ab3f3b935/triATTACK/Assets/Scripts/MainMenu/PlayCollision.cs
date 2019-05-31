using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayCollision : MonoBehaviour
{
    [SerializeField] Transform hitParticle;
    [SerializeField] Transform flashObj;
    [SerializeField] Transform selectUI;
    Text playText;
    MainMenu mainMenu;
    BoxCollider2D col;
    ScreenShake shake;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        col = gameObject.GetComponent<BoxCollider2D>();
        playText = gameObject.GetComponent<Text>();
        mainMenu = gameObject.transform.parent.GetComponent<MainMenu>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(flashObj, transform.position, Quaternion.identity);
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            playText.enabled = false;
            col.enabled = false;
            selectUI.GetComponent<BulletSelect>().DisableUI();
            mainMenu.StartGame();
        }
    }
}
