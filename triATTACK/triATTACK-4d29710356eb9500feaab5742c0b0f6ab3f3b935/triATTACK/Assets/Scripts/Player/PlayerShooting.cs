using System.Collections;
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

        // Add to this when adding more bullet types
        switch (bullets)
        {
            case 0:
                bulletInt = 0;
                break;
            case (Bullets)1:
                bulletInt = 1;
                break;
            default:
                Debug.LogError("No bullet found in enumeration.");
                break;
        }
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
                FindObjectOfType<AudioManager>().Play("PlayerRecall");
                //FindBullet();
                //GameObject.FindGameObjectWithTag("TriBullet").GetComponent<Bullet>().SetIsRecalling(true);
                instBullet.GetComponent<Bullet>().SetIsRecalling(true);
                canRecall = false;
                isRecalling = true;
            }
        }
    }

    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("PlayerShoot");
        instBullet = Instantiate(bulletPrefabs[bulletInt], firePoint.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
    }

    void FindBullet()
    {
        switch (bullets)
        {
            case 0:
                GameObject.FindGameObjectWithTag("TriBullet").GetComponent<Bullet>().SetIsRecalling(true);
                break;
            case (Bullets)1:
                // Get SetIsRecalling of TripleBullet script and set to true
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
            trail.time = .5f;
            spriteR.sprite = normalSprite;
            shake.Shake(shakeDuration, shakeIntensity * 2f);
            Destroy(other.transform.parent.gameObject);
            instBullet = null;
            canRecall = false;
            isRecalling = false;
            // Animation for recharging?
            FindObjectOfType<AudioManager>().Play("PlayerTriHit");
            FindObjectOfType<ComboUI>().ResetCounter();
            Invoke("CanShoot", .25f);
        }
    }
}
