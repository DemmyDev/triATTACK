using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting: MonoBehaviour {

    public float fireRate;
    public int damage;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;

    private Transform firePointOne;
    private Transform firePointTwo;
    private Transform firePointThree;

    private float timeToFire = 0;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    void Start()
    {
        firePointOne = transform.Find("BulletSpawn1");
        firePointTwo = transform.Find("BulletSpawn2");
        firePointThree = transform.Find("BulletSpawn3");
        shake = Camera.main.GetComponent<ScreenShake>();
    }

    void Update ()
    {
        if (!PauseMenu.isPaused)
        {
            if (fireRate == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                }
            }
            else
            {
                if (Input.GetMouseButton(0) && Time.time > timeToFire)
                {
                    timeToFire = Time.time + 1 / fireRate;
                    Shoot();
                }
            }
        }
	}

    void Shoot()
    {
        shake.Shake(shakeDuration, shakeIntensity);

        Instantiate(bulletTrailPrefab, firePointOne.position, firePointOne.rotation);
        Instantiate(bulletTrailPrefab, firePointTwo.position, firePointOne.rotation);
        Instantiate(bulletTrailPrefab, firePointThree.position, firePointOne.rotation);
    }
}
