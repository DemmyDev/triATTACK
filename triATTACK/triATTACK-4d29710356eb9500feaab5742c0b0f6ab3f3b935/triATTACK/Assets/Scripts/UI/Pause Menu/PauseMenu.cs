using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [SerializeField] GameObject fadeObj;
    [SerializeField] GameObject resumeButton;
    [SerializeField] EventSystem eventSystem;

    CanvasGroup canvas;
    Animation anim;

    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        anim = fadeObj.GetComponent<Animation>();
    }

    void Update ()
    {
		if ((Input.GetButtonDown("Pause")) && !Player.isDead)
        {
            if (isPaused)
            {
                Resume();
            }
            else if (!GameMaster.isFreezing)
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        Time.timeScale = 1f;
        isPaused = false;
        Player.SetDeathBool(false);
    }

    void Pause()
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
        eventSystem.SetSelectedGameObject(resumeButton);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
        GameMaster.Instance.ResetHealth();
        isPaused = false;
        Time.timeScale = 1f;
        GameMaster.Instance.DisableObjectScripts();
        anim.Play();
        Invoke("LoadMenu", 1f);
    }

    void LoadMenu()
    {
        anim.Stop();
        Player.isDead = false;
        ReadWriteSaveManager.Instance.SetData("BulletType", 0, true);
        AudioManager.Instance.Stop("MusicGame");
        LoadingManager.Instance.LoadScene("MainMenu", gameObject.scene);
    }

    public void RestartGame()
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
        Time.timeScale = 1f;
        anim.Play();
        GameMaster.Instance.DisableObjectScripts();
        GameMaster.Instance.Invoke("RestartScene", 1f);
        Invoke("Resume", 1f);        
    }
}