﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject deathText;
    public GameObject scoreText;
    public GameObject healthUI;

    public Transform playerParticlePrefab;
    
    public int health;
    public int numOfTris;

    public Image[] tris;
    public Sprite fullTri;
    public Sprite emptyTri;

    public float screenX;
    public float screenY;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    public static bool isInvincible = false;
    public static bool isDead = false;
    private Animation anim;
    private SpriteRenderer sprite;

    private void Start()
    {
        anim = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        if (health > numOfTris)
        {
            health = numOfTris;
        }

        for (int i = 0; i < tris.Length; i++)
        {
            if (i < health)
            {
                tris[i].sprite = fullTri;
            }
            else
            {
                tris[i].sprite = emptyTri;
            }

            if (i < numOfTris)
            {
                tris[i].enabled = true;
            }
            else
            {
                tris[i].enabled = false;
            }
        }

        
        Vector2 newPos = transform.position;

        if (transform.position.x > screenX)
        {
            newPos.x = -screenX;
        }
        if (transform.position.x < -screenX)
        {
            newPos.x = screenX;
        }
        if (transform.position.y > screenY)
        {
            newPos.y = -screenY;
        }
        if (transform.position.y < -screenY)
        {
            newPos.y = screenY;
        }
        transform.position = newPos;
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            DamagePlayer(1);
        }
    }

    public void DamagePlayer(int damage)
    {
        if (!isInvincible)
        {
            Instantiate(playerParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            shake.Shake(shakeDuration, shakeIntensity);

            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                GameMaster gm = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
                gm.DeleteObjectsOnPlayerDeath();

                DeathText dText = deathText.GetComponent<DeathText>();
                ScoreText sText = scoreText.GetComponent<ScoreText>();
                HealthUI hUI = healthUI.GetComponent<HealthUI>();
                hUI.DisableUI();
                dText.EnableText();
                sText.MoveText();
                gameObject.SetActive(false);
            }

            anim.enabled = true;
            isInvincible = true;
            Invoke("EndInvincibility", 1);
            
        }
    }

    void EndInvincibility()
    {
        isInvincible = false;
        anim.enabled = false;
        sprite.enabled = true;
    }
	
}
