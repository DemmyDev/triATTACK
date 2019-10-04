using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Indicate : MonoBehaviour
{
    [SerializeField] GameObject highScoreObj;

    void Start()
    {
        //if (ReadWriteSaveManager.Instance.GetData("PlayedOnce", false, false))
        if (true)
        {
            highScoreObj.SetActive(true);
            highScoreObj.GetComponent<Text>().text = "tri.highScore = " + ReadWriteSaveManager.Instance.GetData("HighScore", 0, false).ToString() + ";";

            gameObject.SetActive(false);
        }
        
	}
}
