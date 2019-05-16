﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform[] bulletPrefabs;
    public enum Bullets { Default = 0, Triple = 1 };
    // Bullets enum will be used to assign bullet types from main menu
    public Bullets bullets;
    int bulletInt = 0;

    Transform instBullet;

    Transform firePoint;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    SpriteRenderer spriteR;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite recallSprite;

    TrailRenderer trail;

    bool canShoot, hasShot;
    [HideInInspector] public bool isRecalling, canRecall;

    void Start()
    {
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = normalSprite;
        firePoint = transform.Find("BulletSpawn");
        shake = Camera.main.GetComponent<ScreenShake>();
        canShoot = true;
        canRecall = false;
        hasShot = false;
        isRecalling = false;

        bulletInt = (int)bullets;
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (canShoot && Input.GetMouseButtonDown(0))
            {
                trail.time = 0;
                spriteR.sprite = recallSprite;
                Shoot();
                hasShot = true;
                canShoot = false;
                Invoke("CanRecall", .25f);
            }
            else if (!isRecalling && hasShot && canRecall && Input.GetMouseButtonDown(0))
            {
                AudioManager.am.Play("PlayerRecall");
                FindBullet();
                canRecall = false;
                isRecalling = true;
            }
        }
    }

    void Shoot()
    {
        AudioManager.am.Play("PlayerShoot");
        instBullet = Instantiate(bulletPrefabs[bulletInt], firePoint.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
    }

    void FindBullet()
    {
        switch (bulletInt)
        {
            // Default bullet
            case 0:
                instBullet.GetComponent<Bullet>().rb.velocity = Vector2.zero;
                instBullet.GetComponent<Bullet>().SetIsRecalling(true);
                break;
            // Triple bullet
            case 1:
                instBullet.GetComponent<TripleBullet>().SetIsRecalling(true);
                break;
            default:
                Debug.LogError("Could not find bullet");
                break;
        }
    }

    void CanRecall()
    {
        canRecall = true;
    }

    void CanShoot()
    {
        canShoot = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet") && (isRecalling || canRecall))
        {
            trail.time = .5f; // Need here
            spriteR.sprite = normalSprite; // Need here
            shake.Shake(shakeDuration, shakeIntensity * 2f); // Need here
            Destroy(other.transform.parent.gameObject); // Don't technically need here + needs to change based on bullet type
            instBullet = null; // Need here
            canRecall = false; // Don't technically need here
            isRecalling = false; // Don't technically need here
            // Animation for recharging?
            AudioManager.am.Play("PlayerTriHit"); // Don't need here
            FindObjectOfType<ComboUI>().ResetCounter(); // Don't need here
            Invoke("CanShoot", .25f); // Need here
        }
    }

    public void BulletHit()
    {
        trail.time = .5f;
        spriteR.sprite = normalSprite;
        instBullet = null;
        canRecall = false;
        isRecalling = false;
        FindObjectOfType<ComboUI>().ResetCounter();
        Invoke("CanShoot", .25f);
    }
}
