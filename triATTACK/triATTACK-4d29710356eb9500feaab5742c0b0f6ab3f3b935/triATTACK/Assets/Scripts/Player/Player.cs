using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] Transform playerParticlePrefab;
    
    [SerializeField] int health;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    ScreenShake shake;
    [SerializeField] float shakeDuration;

    public static bool isDead = false;
    TrailRenderer trail;

    float screenX = 36.25f, screenY = 20.75f;

    PlayerShooting shooting;

    void Start()
    {
        shooting = GetComponent<PlayerShooting>();
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
        health -= 1;

        Instantiate(playerParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
        AudioManager.Instance.Play("PlayerHit");
        GameMaster.Instance.Respawn(gameObject);
    }

    public static void SetDeathBool(bool dead)
    {
        isDead = dead;
    }
}
