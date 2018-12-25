using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathText : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameMaster.RestartScene();
        }
    }

    public void EnableText()
    {
        gameObject.SetActive(true);
    }
}