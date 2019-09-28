using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] Transform[] bulletPrefabs;
    public enum Bullets { Default = 0, Triple = 1, Follow = 2, Sponge = 3, Rapid = 4, Bounce = 5 };
    // Bullets enum will be used to assign bullet types from main menu (and the inspector)
    public Bullets bullets;

    Transform instBullet;

    [HideInInspector] public Transform firePoint;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    SpriteRenderer spriteR;
    [SerializeField] Sprite normalSprite;
    [SerializeField] Sprite recallSprite;

    bool canShoot, hasShot;
    [HideInInspector] public bool isRecalling, canRecall;
    [SerializeField] bool enableDevInput;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        spriteR.sprite = normalSprite;
        firePoint = transform.Find("BulletSpawn");
        shake = Camera.main.GetComponent<ScreenShake>();
        canShoot = true;
        canRecall = false;
        hasShot = false;
        isRecalling = false;
        if (SceneManager.GetActiveScene().name == "MainMenu") ReadWriteSaveManager.Instance.SetData("BulletType", 0, true);
        bullets = (Bullets)ReadWriteSaveManager.Instance.GetData("BulletType", 0);
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            if (canShoot && (Input.GetButtonDown("Shoot") || Input.GetAxis("ControlShoot") != 0))
            {
                spriteR.sprite = recallSprite;
                Shoot();
                hasShot = true;
                canShoot = false;
                Invoke("CanRecall", .33f);
            }
            else if (!isRecalling && hasShot && canRecall && (Input.GetButtonDown("Shoot") || Input.GetAxis("ControlShoot") != 0))
            {
                AudioManager.Instance.Play("PlayerRecall");
                FindBullet();
                canRecall = false;
                isRecalling = true;
            }
        }

        // Dev inputs for setting bullet types
        if (enableDevInput)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0)) ReadWriteSaveManager.Instance.SetData("BulletType", 0, true); // Set default
            if (Input.GetKeyDown(KeyCode.Alpha1)) ReadWriteSaveManager.Instance.SetData("BulletType", 1, true); // Set triple
            if (Input.GetKeyDown(KeyCode.Alpha2)) ReadWriteSaveManager.Instance.SetData("BulletType", 2, true); // Set follow
            if (Input.GetKeyDown(KeyCode.Alpha3)) ReadWriteSaveManager.Instance.SetData("BulletType", 3, true); // Set sponge
            if (Input.GetKeyDown(KeyCode.Alpha4)) ReadWriteSaveManager.Instance.SetData("BulletType", 4, true); // Set rapid
            if (Input.GetKeyDown(KeyCode.Alpha5)) ReadWriteSaveManager.Instance.SetData("BulletType", 5, true); // Set bounce
        }
    }

    void Shoot()
    {
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
            case Bullets.Rapid:
                instBullet.GetComponent<RapidBullet>().SetIsRecalling(true);
                break;
            case Bullets.Bounce:
                instBullet.GetComponent<BounceBullet>().SetIsRecalling(true);
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

    public Vector3 GetFirepointPos()
    {
        return firePoint.position;
    }

    public Vector3 GetPlayerRot()
    {
        return transform.eulerAngles;
    }

    public void BulletHit()
    {
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
        else if (bullets != (Bullets)ReadWriteSaveManager.Instance.GetData("BulletType", 0))
        {
            bullets = (Bullets)ReadWriteSaveManager.Instance.GetData("BulletType", 0);
        }
    }
}
