using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownToStart : MonoBehaviour {

    Text text;
    float mainTimer = 3f;

    float timer;
    bool timeIsZero = false;
    GameMaster gm;

    Animation anim;

	void Start ()
    {
        anim = gameObject.GetComponent<Animation>();
        gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        gm.DisableObjectScripts();
        text = GetComponent<Text>();
        timer = mainTimer;

        PlayCountdownSound();
        Invoke("PlayCountdownSound", .97f);
        Invoke("PlayCountdownSound", 1.97f);
    }

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            text.text = "tri.TimeUntilStart = " + timer.ToString("F1") + ";";
        }
        else if (timer <= 0.09f && !timeIsZero)
        {
            FindObjectOfType<AudioManager>().Play("GameStart");
            timeIsZero = true;
            text.text = "tri.Begin();";
            gm.StartGame();
            Invoke("FadeOut", 1f);
        }
    }

    void PlayCountdownSound()
    {
        FindObjectOfType<AudioManager>().Play("Countdown");
        Debug.Log(timer);
    }

    void FadeOut()
    {
        anim.Play();
        Invoke("Disable", .5f);
    }

    void Disable()
    {
        anim.Stop();
        gameObject.SetActive(false);
    }
}
