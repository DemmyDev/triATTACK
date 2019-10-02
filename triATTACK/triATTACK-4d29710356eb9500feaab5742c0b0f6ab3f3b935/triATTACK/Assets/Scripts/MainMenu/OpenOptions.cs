﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenOptions : MonoBehaviour
{
    [SerializeField] GameObject plainText;
    [SerializeField] Transform hitParticle;
    [SerializeField] GameObject optionsMenu;
    ScreenShake shake;

    GameObject playButton;

    void Start()
    {
        playButton = transform.parent.gameObject;
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            shake.Shake(.1f, .5f);
            AudioManager.Instance.Play("TextEnter");
            Instantiate(hitParticle, transform.position, Quaternion.identity);

            plainText.SetActive(false);
            optionsMenu.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
    }
}