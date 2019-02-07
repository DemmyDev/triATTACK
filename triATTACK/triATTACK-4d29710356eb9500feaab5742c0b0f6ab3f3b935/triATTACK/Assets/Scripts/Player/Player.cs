using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    [SerializeField] GameObject deathText;
    [SerializeField] GameObject scoreText;
    [SerializeField] GameObject healthUI;

    [SerializeField] Transform playerParticlePrefab;
    
    [SerializeField] int health;
    [SerializeField] int numOfTris;

    [SerializeField] Image[] tris;
    [SerializeField] Sprite fullTri;
    [SerializeField] Sprite emptyTri;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    ScreenShake shake;
    [SerializeField] float shakeDuration;

    public static bool isInvincible = false;
    public static bool isDead = false;
    Animation anim;
    SpriteRenderer sprite;

    void Start()
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

            anim.Play();
            isInvincible = true;
            Invoke("EndInvincibility", 1);
        }
    }

    void EndInvincibility()
    {
        isInvincible = false;
        anim.Stop();
        sprite.enabled = true;
    }

    public static void ResetDeathBool()
    {
        isDead = false;
    }
}
