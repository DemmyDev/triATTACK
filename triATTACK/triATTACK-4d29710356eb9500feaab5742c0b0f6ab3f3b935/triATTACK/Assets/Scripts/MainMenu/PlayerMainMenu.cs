using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainMenu : MonoBehaviour
{
    [SerializeField] Transform bulletTrailPrefab;

    Transform firePoint;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    TrailRenderer trail;

    float screenX = 36.25f, screenY = 20.75f;

    SpriteRenderer spriteR;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite recallSprite;

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

    void Update()
    {
        ScreenWrap();

        if (canShoot && Input.GetMouseButton(0))
        {
            spriteR.sprite = recallSprite;
            trail.time = 0;
            Shoot();
            hasShot = true;
            canShoot = false;
            Invoke("CanRecall", .25f);
        }
        else if (hasShot && canRecall && Input.GetMouseButton(0))
        {
            FindObjectOfType<AudioManager>().Play("PlayerRecall");
            Bullet.isRecalling = true;
            isRecalling = true;
            canRecall = false;
        }
    }

    void ScreenWrap()
    {
        Vector2 pos = transform.position;

        if (pos.x > screenX)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(-screenX, pos.y);
        }

        if (pos.x < -screenX)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(screenX, pos.y);
        }

        if (pos.y > screenY)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, -screenY);
        }

        if (pos.y < -screenY)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, screenY);
        }
    }

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.1f);
        trail.time = .5f;
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
            FindObjectOfType<AudioManager>().Play("PlayerTriHit");
            spriteR.sprite = normalSprite;
            trail.time = .5f;
            shake.Shake(shakeDuration, shakeIntensity * 2f);
            Destroy(other.transform.parent.gameObject);
            Bullet.isRecalling = false;
            isRecalling = false;
            canRecall = false;
            // Animation for recharging?
            Invoke("CanShoot", .25f);
        }
    }
}
