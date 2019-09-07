using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform[] bulletPrefabs;
    public enum Bullets { Default = 0, Triple = 1, Follow = 2, Sponge = 3 };
    // Bullets enum will be used to assign bullet types from main menu (and the inspector)
    public Bullets bullets;

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
        bullets = (Bullets)PlayerPrefs.GetInt("BulletType", 0);
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (canShoot && Input.GetButtonDown("Shoot"))
            {
                trail.time = 0;
                spriteR.sprite = recallSprite;
                Shoot();
                hasShot = true;
                canShoot = false;
                Invoke("CanRecall", .25f);
            }
            else if (!isRecalling && hasShot && canRecall && Input.GetButtonDown("Shoot"))
            {
                AudioManager.Instance.Play("PlayerRecall");
                FindBullet();
                canRecall = false;
                isRecalling = true;
            }
        }
    }

    void Shoot()
    {
        AudioManager.Instance.Play("PlayerShoot");
        instBullet = Instantiate(bulletPrefabs[(int)bullets], firePoint.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
    }

    void FindBullet()
    {
        switch (bullets)
        {
            case Bullets.Default:
                instBullet.GetComponent<Bullet>().rb.velocity = Vector2.zero;
                instBullet.GetComponent<Bullet>().SetIsRecalling(true);
                break;
            case Bullets.Triple:
                instBullet.GetComponent<TripleBullet>().SetIsRecalling(true);
                break;
            case Bullets.Follow:
                instBullet.GetComponent<FollowBullet>().SetIsRecalling(true);
                break;
            case Bullets.Sponge:
                instBullet.GetComponent<SpongeBullet>().SetIsRecalling(true);
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

    public bool GetHasShot()
    {
        return hasShot;
    }

    public void BulletHit()
    {
        trail.time = .75f;
        spriteR.sprite = normalSprite;
        instBullet = null;
        canRecall = false;
        isRecalling = false;
        hasShot = false;
        Invoke("CanShoot", .25f);

        if (SceneManager.GetActiveScene().name == "tri.Attack")
        {
            FindObjectOfType<ComboUI>().ResetCounter();
        }
        else if (bullets != (Bullets)PlayerPrefs.GetInt("BulletType", 0))
        {
            bullets = (Bullets)PlayerPrefs.GetInt("BulletType", 0);
        }
    }
}
