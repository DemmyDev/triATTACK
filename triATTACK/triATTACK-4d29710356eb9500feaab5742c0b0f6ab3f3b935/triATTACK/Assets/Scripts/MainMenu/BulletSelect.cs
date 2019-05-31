using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelect : MonoBehaviour {

    [SerializeField] Text[] childUI;

    void Start()
    {
        CheckUI();
    }

    public void SelectBullet(int bulletNum)
    {
        PlayerPrefs.SetInt("BulletType", bulletNum);
        CheckUI();
    }

    void CheckUI()
    {
        switch (PlayerPrefs.GetInt("BulletType", 0))
        {
            // Normal
            case 0:
                childUI[0].CrossFadeAlpha(.33f, 0, true);
                childUI[1].CrossFadeAlpha(1f, 0, true);
                childUI[2].CrossFadeAlpha(1f, 0, true);
                break;
            // Triple
            case 1:
                childUI[0].CrossFadeAlpha(1f, 0, true);
                childUI[1].CrossFadeAlpha(.33f, 0, true);
                childUI[2].CrossFadeAlpha(1f, 0, true);
                break;
            // Follow
            case 2:
                childUI[0].CrossFadeAlpha(1f, 0, true);
                childUI[1].CrossFadeAlpha(1f, 0, true);
                childUI[2].CrossFadeAlpha(.33f, 0, true);
                break;
            default:
                Debug.LogError("Bullet type int not within proper bounds");
                break;
        }
    }

    public void DisableUI()
    {
        foreach (Text child in childUI) child.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
