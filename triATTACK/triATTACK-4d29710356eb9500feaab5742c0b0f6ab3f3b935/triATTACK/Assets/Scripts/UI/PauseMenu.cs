using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;

    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] GameObject fadeObj;
    Animation anim;

    GlitchEffect glitch;

    void Start()
    {
        anim = fadeObj.GetComponent<Animation>();
        glitch = Camera.main.GetComponent<GlitchEffect>();
    }

    void Update ()
    {
		if (Input.GetKeyDown(KeyCode.Escape) && !Player.isDead)
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
	}

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        glitch.enabled = false;
        Time.timeScale = 1f;
        isPaused = false;
        Player.ResetDeathBool();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMainMenu()
    {
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
        LoadingManager.Instance.LoadScene("MainMenu", gameObject.scene);
    }

    public void RestartGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        anim.Play();
        //glitch.enabled = true;
        GameMaster.Instance.DisableObjectScripts();
        GameMaster.Instance.Invoke("RestartScene", 1f);
        Invoke("Resume", 1f);        
    }
}