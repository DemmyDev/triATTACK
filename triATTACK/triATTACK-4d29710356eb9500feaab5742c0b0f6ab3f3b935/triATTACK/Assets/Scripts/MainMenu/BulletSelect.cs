using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelect : MonoBehaviour {

    [SerializeField] Text[] selectButtons;
    [SerializeField] GameObject startButton;

    void Start()
    {
        startButton.SetActive(false);
    }

    public void SelectBullet(int bulletNum)
    {
        PlayerPrefs.SetInt("BulletType", bulletNum);
        CheckUI();
        selectButtons[bulletNum].gameObject.GetComponent<Selector>().DisableCollider();
        startButton.SetActive(true);
    }

    public void CheckUI()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("BulletType", 0))
            {
                selectButtons[i].CrossFadeAlpha(.33f, 0, true);
                selectButtons[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                selectButtons[i].CrossFadeAlpha(1f, 0, true);
                selectButtons[i].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }

    public void DisableColliders()
    {
        foreach (Text button in selectButtons) button.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}