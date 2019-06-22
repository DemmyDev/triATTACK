using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectToMenu : MonoBehaviour {

    BoxCollider2D col;
    ScreenShake shake;
    List<GameObject> textsToEnable;
    GameObject select;

	// Use this for initialization
	void Start ()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    public void PassValues(List<GameObject> texts, GameObject selectButton, GameObject selectionUI)
    {
        textsToEnable = texts;
        textsToEnable.Add(selectButton);
        select = selectionUI;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            foreach (GameObject text in textsToEnable) text.SetActive(true);

            select.SetActive(false);

            shake.Shake(.1f, .5f);
            gameObject.SetActive(false);
        }
    }
}
