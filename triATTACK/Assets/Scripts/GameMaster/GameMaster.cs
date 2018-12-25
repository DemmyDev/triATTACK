using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    void Start()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    public static void RestartScene()
    {
        SceneManager.LoadScene("triATTACK");
    }
}
