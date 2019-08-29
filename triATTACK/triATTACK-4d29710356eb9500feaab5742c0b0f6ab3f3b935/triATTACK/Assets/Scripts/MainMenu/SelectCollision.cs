using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCollision : MonoBehaviour
{
    [SerializeField] List<GameObject> textsToDisable;
    [SerializeField] SelectToMenu toMenu;
    [SerializeField] GameObject selectUI;
    BoxCollider2D col;
    ScreenShake shake;

	void Start ()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            foreach (GameObject text in textsToDisable) text.SetActive(false);

            selectUI.SetActive(true);
            selectUI.GetComponent<BulletSelect>().CheckUI();
            toMenu.gameObject.SetActive(true);

            toMenu.PassValues(textsToDisable, gameObject, selectUI);

            shake.Shake(.1f, .5f);
            gameObject.SetActive(false);
        }
    }
}
