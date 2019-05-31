using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    TrailRenderer trail;

    float screenX = 36.25f, screenY = 20.75f;

    PlayerShooting shooting;

    void Start()
    {
        shooting = GetComponent<PlayerShooting>();
        anim = GetComponent<Animation>();
        sprite = GetComponent<SpriteRenderer>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        ScreenWrap();

        if (health > numOfTris)
        {
            health = numOfTris;
        }

        if (SceneManager.GetActiveScene().name == "tri.Attack")
        {
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
    }

    void ScreenWrap()
    {
        Vector2 pos = transform.position;

        if (pos.x > screenX)
        {
            if (!shooting.GetHasShot())
            {
                trail.time = 0;
                StartCoroutine(ResetTrail());
            }
            transform.position = new Vector2(-screenX, pos.y);
        }

        if (pos.x < -screenX)
        {
            if (!shooting.GetHasShot())
            {
                trail.time = 0;
                StartCoroutine(ResetTrail());
            }
            transform.position = new Vector2(screenX, pos.y);
        }

        if (pos.y > screenY)
        {
            if (!shooting.GetHasShot())
            {
                trail.time = 0;
                StartCoroutine(ResetTrail());
            }
            transform.position = new Vector2(pos.x, -screenY);
        }

        if (pos.y < -screenY)
        {
            if (!shooting.GetHasShot())
            {
                trail.time = 0;
                StartCoroutine(ResetTrail());
            }
            transform.position = new Vector2(pos.x, screenY);
        }
    }

    IEnumerator ResetTrail()
    {
        yield return new WaitForSeconds(.1f);
        trail.time = .5f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            DamagePlayer();
        }
    }

    public void DamagePlayer()
    {
        if (!isInvincible)
        {
            Instantiate(playerParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            shake.Shake(shakeDuration, shakeIntensity);

            health -= 1;
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
                sText.SetHighScore();
                gameObject.SetActive(false);
            }

            FindObjectOfType<AudioManager>().Play("PlayerHit");
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
