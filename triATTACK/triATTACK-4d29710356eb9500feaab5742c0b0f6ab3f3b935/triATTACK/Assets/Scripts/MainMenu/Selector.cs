﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

    [SerializeField] int bulletNum;
    [SerializeField] Transform hitParticle;
    BulletSelect parentUI;

    BoxCollider2D col;
    ScreenShake shake;
    Text selectText;

    void Start()
    {
        parentUI = transform.parent.GetComponent<BulletSelect>();
        col = GetComponent<BoxCollider2D>();
        shake = Camera.main.GetComponent<ScreenShake>();
        selectText = GetComponent<Text>();
    }

    public void DisableCollider()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(hitParticle, transform.position, Quaternion.identity);
            selectText.enabled = false;
            col.enabled = false;

            parentUI.SelectBullet(bulletNum);
        }
    }
}