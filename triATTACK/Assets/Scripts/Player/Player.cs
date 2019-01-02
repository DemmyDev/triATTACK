using System.Collections;
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

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    private void Start()
    {
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
        Instantiate(playerParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);

        health -= damage;
        if (health <= 0)
        {
            GameObject.Find("GameMaster").GetComponent<EnemySpawner>().enabled = false;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies)
            {
                GameObject.Destroy(enemy);
            }

            GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
            foreach (GameObject bullet in bullets)
            {
                GameObject.Destroy(bullet);
            }

            GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
            foreach (GameObject particle in particles)
            {
                GameObject.Destroy(particle);
            }
            
            DeathText dText = deathText.GetComponent<DeathText>();
            ScoreText sText = scoreText.GetComponent<ScoreText>();
            HealthUI hUI = healthUI.GetComponent<HealthUI>();
            hUI.DisableUI();
            dText.EnableText();
            sText.MoveText();
            gameObject.SetActive(false);
        }
    }
	
}
