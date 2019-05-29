using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownToStart : MonoBehaviour {

    Text text;
    float mainTimer = 3f;

    float timer;
    bool timeIsZero = false;

    Animation anim;

	void Start ()
    {
        anim = gameObject.GetComponent<Animation>();
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
            AudioManager.Instance.Play("GameStart");
            timeIsZero = true;
            text.text = "tri.Begin();";
            GameMaster.Instance.StartGame();
            Invoke("FadeOut", 1f);
        }
    }

    void PlayCountdownSound()
    {
        AudioManager.Instance.Play("CountdownToStart");
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
