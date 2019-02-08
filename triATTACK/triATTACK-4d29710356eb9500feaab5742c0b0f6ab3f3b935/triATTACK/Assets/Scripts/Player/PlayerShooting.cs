using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting: MonoBehaviour {

    [SerializeField] float fireRate;
    [SerializeField] int damage;

    [SerializeField] Transform bulletTrailPrefab;

    Transform firePoint;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] GameObject trail;

    SpriteRenderer spriteR;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite recallSprite;

    bool canShoot, hasShot, isRecalling;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = normalSprite;
        firePoint = transform.Find("BulletSpawn");
        shake = Camera.main.GetComponent<ScreenShake>();
        canShoot = true;
        hasShot = false;
        isRecalling = false;
    }

    void Update ()
    {
        if (!PauseMenu.isPaused)
        {
            if (canShoot && Input.GetMouseButton(0))
            {
                spriteR.sprite = recallSprite; 
                trail.SetActive(false);
                Shoot();
                hasShot = true;
                canShoot = false;
                Debug.Log("Shoot");
            }
            else if (hasShot && Input.GetKeyDown(KeyCode.Space)) // Change spacebar to right mouse
            {
                Bullet.isRecalling = true;
                isRecalling = true;
                Debug.Log("Recall");
            }
        }
    }

    void Shoot()
    {
        Instantiate(bulletTrailPrefab, firePoint.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
    }

    void CanShoot()
    {
        canShoot = true;
        Debug.Log("Can shoot");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet") && isRecalling)
        {
            spriteR.sprite = normalSprite;
            trail.SetActive(true);
            shake.Shake(shakeDuration, shakeIntensity * 2f);
            Destroy(other.transform.parent.gameObject);
            Bullet.isRecalling = false;
            isRecalling = false;
            // Animation for recharging?
            Invoke("CanShoot", .25f);
        }
    }
}
