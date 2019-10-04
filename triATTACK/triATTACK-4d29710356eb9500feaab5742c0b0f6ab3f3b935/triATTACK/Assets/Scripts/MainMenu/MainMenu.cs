using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu: MonoBehaviour {

    [SerializeField] GameObject fadeObj;
    [SerializeField] bool wipeData;
    Animation anim;

    void Start()
    {
        ReadWriteSaveManager.Instance.SetData("BulletType", 0, true);
        if (ReadWriteSaveManager.Instance.GetData("MusicOn", true, false)) AudioManager.Instance.Play("MusicMenu");
        anim = fadeObj.GetComponent<Animation>();
        if (wipeData) ReadWriteSaveManager.Instance.Wipe();
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
        AudioManager.Instance.Stop("MusicMenu");
        anim.Stop();
        LoadingManager.Instance.LoadScene("tri.Attack", gameObject.scene);
    }
}
