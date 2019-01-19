using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting: MonoBehaviour {

    public float fireRate;
    public int damage;
    public LayerMask whatToHit;

    public Transform bulletTrailPrefab;

    private Transform firePointOne;
    private Transform firePointTwo;
    private Transform firePointThree;

    private float timeToFire = 0;

    public Slider triAttackMeterUI;

    public float triAttackActivateValue;
    public float triAttackMaxValue;
    public float triAttackMeter = 0f;
    private bool triAttackIsActive = false;
    private bool isAtMaxValue = false;

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

        triAttackActivateValue = triAttackMaxValue / 2;
        triAttackMeterUI.maxValue = triAttackMaxValue;
    }

    void Update ()
    {
        if (!PauseMenu.isPaused)
        {
            if (triAttackIsActive)
            {
                triAttackMeter -= Time.deltaTime * 2f;

                if (triAttackMeter < 0f)
                {
                    Debug.Log("tri attack finished");
                    triAttackMeter = 0f;
                    triAttackIsActive = false;
                }
            }
            else
            {
                if (triAttackMeter >= triAttackMaxValue && !isAtMaxValue)
                {
                    Debug.Log("tri attack at max value ");
                    triAttackMeter = triAttackMaxValue;
                    isAtMaxValue = true;
                }

                if (!isAtMaxValue)
                {
                    triAttackMeter += Time.deltaTime;
                }

                if (triAttackMeter >= triAttackActivateValue)
                {
                    Debug.Log("tri attack can be activated");
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("tri attack activated");
                        triAttackIsActive = true;
                        isAtMaxValue = false;
                    }
                }
            }

            triAttackMeterUI.value = triAttackMeter;

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
        Instantiate(bulletTrailPrefab, firePointOne.position, firePointOne.rotation);

        if (triAttackIsActive)
        {
            Instantiate(bulletTrailPrefab, firePointTwo.position, firePointOne.rotation);
            Instantiate(bulletTrailPrefab, firePointThree.position, firePointOne.rotation);
            shake.Shake(shakeDuration, shakeIntensity * 2);
        }
        else
        {
            shake.Shake(shakeDuration, shakeIntensity);
        }

    }
}
