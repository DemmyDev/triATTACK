﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownToStart : MonoBehaviour {

    Text text;
    float mainTimer = 3f;

    float timer;
    bool timeIsZero = false;
    GameMaster gm;

	void Start ()
    {
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
            Invoke("Disable", 1f);
        }
    }

    void Disable()
    {
        gameObject.SetActive(false);        
    }
}