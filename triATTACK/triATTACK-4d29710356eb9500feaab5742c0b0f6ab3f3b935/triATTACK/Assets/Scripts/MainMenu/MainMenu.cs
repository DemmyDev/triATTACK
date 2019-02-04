using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    public void StartGame()
    {
        anim.Play();
        Invoke("LoadGame", 1f);
    }

    public void QuitGame()
    {
        Debug.Log("please end me i'm in pain");
        Application.Quit();
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
