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
	}

    void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
            text.text = "tri.TimeUntilStart = " + timer.ToString("F2") + ";";
            
        }
        else if (timer <= 0.01f && !timeIsZero)
        {
            timeIsZero = true;
            text.text = "tri.Begin();";
            gm.StartGame();
            Invoke("FadeOut", 1f);
        }
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
