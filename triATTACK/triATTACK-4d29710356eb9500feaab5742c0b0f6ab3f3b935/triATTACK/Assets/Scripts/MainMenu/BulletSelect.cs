using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelect : MonoBehaviour {

    [SerializeField] Text[] selectButtons;
    MainMenu mainMenu;

    void Start()
    {
        mainMenu = transform.parent.GetComponent<MainMenu>();
    }

    public void SelectBullet(int bulletNum)
    {
        PlayerPrefs.SetInt("BulletType", bulletNum);
        CheckUI();
        foreach (Text button in selectButtons) button.gameObject.GetComponent<Selector>().DisableCollider();
        mainMenu.StartGame();
    }

    public void CheckUI()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("BulletType", 0)) selectButtons[i].CrossFadeAlpha(.33f, 0, true);
            else selectButtons[i].CrossFadeAlpha(1f, 0, true);
        }
    }
}
