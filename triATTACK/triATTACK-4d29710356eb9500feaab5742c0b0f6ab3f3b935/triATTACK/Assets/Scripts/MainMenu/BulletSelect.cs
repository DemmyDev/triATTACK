using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelect : MonoBehaviour {

    [SerializeField] Text[] selectButtons;
    [SerializeField] GameObject startButton;
    PlayerShooting player;

    void Start()
    {
        startButton.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShooting>();
    }

    public void SelectBullet(int bulletNum)
    {
        player.ForceRecall();
        ReadWriteSaveManager.Instance.SetData("BulletType", bulletNum, true);
        CheckUI();
        selectButtons[bulletNum].gameObject.GetComponent<Selector>().DisableCollider();
        startButton.SetActive(true);
    }

    public void CheckUI()
    {
        for (int i = 0; i < selectButtons.Length; i++)
        {
            // Selected button
            if (i == ReadWriteSaveManager.Instance.GetData("BulletType", 0))
            {
                selectButtons[i].CrossFadeAlpha(.5f, 0, true);
                selectButtons[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            // Unlocked and unselected button
            else if (selectButtons[i].gameObject.GetComponent<Selector>().GetIsUnlocked())
            {
                selectButtons[i].CrossFadeAlpha(1f, 0, true);
                selectButtons[i].gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            // Not unlocked
            else
            {
                selectButtons[i].CrossFadeAlpha(.25f, 0, true);
                selectButtons[i].gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void DisableColliders()
    {
        foreach (Text button in selectButtons) button.gameObject.GetComponent<Selector>().DisableCollider();
    }
}