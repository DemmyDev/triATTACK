using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Transform playerParticlePrefab;
    [SerializeField] GameObject screenwrapPrefab;

    [SerializeField] int health;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    ScreenShake shake;
    [SerializeField] float shakeDuration;

    public static bool isDead = false;
    [SerializeField] TrailRenderer trail;

    float screenX = 36.25f, screenY = 20.75f;

    PlayerShooting shooting;

    void Start()
    {
        shooting = GetComponent<PlayerShooting>();
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
            if (!shooting.GetHasShot()) StartCoroutine(ResetTrail());
            transform.position = new Vector2(-screenX, pos.y);
            var inst = Instantiate(screenwrapPrefab, new Vector2(-screenX + 1, pos.y), transform.rotation);
            Destroy(inst, .5f);
            AudioManager.Instance.Play("Screenwrap");
        }

        if (pos.x < -screenX)
        {
            if (!shooting.GetHasShot()) StartCoroutine(ResetTrail());
            transform.position = new Vector2(screenX, pos.y);
            var inst = Instantiate(screenwrapPrefab, new Vector2(screenX - 1, pos.y), transform.rotation);
            Destroy(inst, .5f);
            AudioManager.Instance.Play("Screenwrap");
        }

        if (pos.y > screenY)
        {
            if (!shooting.GetHasShot()) StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, -screenY);
            var inst = Instantiate(screenwrapPrefab, new Vector2(pos.x, -screenY + .75f), transform.rotation);
            Destroy(inst, .5f);
            AudioManager.Instance.Play("Screenwrap");
        }

        if (pos.y < -screenY)
        {
            if (!shooting.GetHasShot()) StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, screenY);
            var inst = Instantiate(screenwrapPrefab, new Vector2(pos.x, screenY - .75f), transform.rotation);
            Destroy(inst, .5f);
            AudioManager.Instance.Play("Screenwrap");
        }
    }

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.15f);
        trail.time = .75f;
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
