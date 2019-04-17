using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting: MonoBehaviour
{
    [SerializeField] Transform bulletTrailPrefab;

    Transform firePoint;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    SpriteRenderer spriteR;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite recallSprite;

    TrailRenderer trail;

    bool canShoot, canRecall, hasShot, isRecalling;

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
    }

    void Update ()
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
                FindObjectOfType<AudioManager>().Play("PlayerRecall");
                GameObject.FindGameObjectWithTag("TriBullet").GetComponent<Bullet>().SetIsRecalling(true);
                canRecall = false;
                isRecalling = true;
            }
        }
    }

    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("PlayerShoot");
        Instantiate(bulletTrailPrefab, firePoint.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
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
            trail.time = .5f;
            spriteR.sprite = normalSprite;
            shake.Shake(shakeDuration, shakeIntensity * 2f);
            Destroy(other.transform.parent.gameObject);
            canRecall = false;
            isRecalling = false;
            // Animation for recharging?
            FindObjectOfType<AudioManager>().Play("PlayerTriHit");
            FindObjectOfType<ComboUI>().ResetCounter();
            Invoke("CanShoot", .25f);
        }
    }
}
