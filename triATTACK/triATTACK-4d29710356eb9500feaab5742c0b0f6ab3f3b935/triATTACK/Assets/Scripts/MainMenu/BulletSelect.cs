using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletSelect : MonoBehaviour {

    [SerializeField] Text[] childUI;

    public void SelectBullet(int bulletNum)
    {
        PlayerPrefs.SetInt("BulletType", bulletNum);
        CheckUI();
    }

    public void CheckUI()
    {
        for (int i = 0; i < childUI.Length; i++)
        {
            if (i == PlayerPrefs.GetInt("BulletType", 0)) childUI[i].CrossFadeAlpha(.33f, 0, true);
            else childUI[i].CrossFadeAlpha(1f, 0, true);
        }
    }
}
