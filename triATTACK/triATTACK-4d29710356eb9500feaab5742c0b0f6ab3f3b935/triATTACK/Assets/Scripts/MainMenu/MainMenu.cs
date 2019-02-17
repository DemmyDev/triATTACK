using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu: MonoBehaviour {

    [SerializeField] GameObject fadeObj;
    Animation anim;

    void Start()
    {
        anim = fadeObj.GetComponent<Animation>();
    }

    public void StartGame()
    {
        fadeObj.SetActive(true);
        anim.Play();
        Debug.Log("Start Game");
        Invoke("LoadGame", 1f);
    }

    public void QuitGame()
    {
        Debug.Log("please end me i'm in pain");
        Application.Quit();
    }

    public void LoadGame()
    {
        anim.Stop();
        Debug.Log("Load Game");
        SceneManager.LoadScene(1);
    }
}
