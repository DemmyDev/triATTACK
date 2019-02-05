using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting: MonoBehaviour {

    [SerializeField] float fireRate;
    [SerializeField] int damage;

    [SerializeField] Transform bulletTrailPrefab;

    Transform firePoint;

    float timeToFire = 0;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    bool canShoot, hasShot, isRecalling;

    void Start()
    {
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
            // If the player can shoot (canShoot == true) && if the player hits left mouse 
                // Shoot function (instantiate object at firepoint, screenshake)
                // hasShot is true, canShoot is false
            // If the player has shot (hasShot == true) && if the player hits right mouse
                // Recall function (public void that changes bool in Bullet to make bullet go to player)

            // On collision with bullet
                // Destroy bullet 
                // canShoot becomes true (cooldown for canShoot to become true?), hasShot becomes false
                
            if (canShoot && Input.GetMouseButton(0))
            {
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
            Destroy(other.gameObject);
            Bullet.isRecalling = false;
            isRecalling = false;
            // Animation for recharging?
            Invoke("CanShoot", .5f);
        }
    }
}
