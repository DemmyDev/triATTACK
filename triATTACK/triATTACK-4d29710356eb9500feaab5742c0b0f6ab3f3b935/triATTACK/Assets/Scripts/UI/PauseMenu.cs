using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;

    public GameObject pauseMenuUI;

    private GlitchEffect glitch;

    private void Start()
    {
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
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        glitch.enabled = true;
        GameMaster gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        gm.DisableObjectScripts();
        gm.Invoke("RestartScene", 1f);
        Invoke("Resume", 1f);        
    }
}