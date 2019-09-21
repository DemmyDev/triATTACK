using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] Transform hitParticle;
    BoxCollider2D col;
    ScreenShake shake;
    MainMenu mainMenu;
    BulletSelect parentUI;

    void Start()
    {
        parentUI = transform.parent.GetComponent<BulletSelect>();
        mainMenu = parentUI.transform.parent.GetComponent<MainMenu>();
        shake = Camera.main.GetComponent<ScreenShake>();
        col = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            col.enabled = false;
            parentUI.DisableColliders();

            mainMenu.StartGame();
            gameObject.SetActive(false);
        }
    }
}