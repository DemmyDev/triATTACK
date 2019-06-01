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
        Invoke("LoadGame", 1f);
    }

    public void QuitGame()
    {
        anim.Play();
        Invoke("Quit", 1f);
    }

    void Quit()
    {
        Debug.Log("please end me i'm in pain");
        Application.Quit();
    }

    public void LoadGame()
    {
        anim.Stop();
        LoadingManager.Instance.LoadScene("tri.Attack", gameObject.scene);
    }
}
