using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingEnemy : MonoBehaviour
{
    [SerializeField] float minRotateSpeed;
    [SerializeField] float maxRotateSpeed;
    float currentRotateSpeed = 0;
    float rotateSpeed;
    Transform target;
    float rateOfFire, startRateOfFire;

    [SerializeField] GameObject bulletTrailPrefab;

    [SerializeField] Transform sittingParticlePrefab;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] int health;

    bool indicateShoot = false;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);

        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }

        int randNum = Random.Range(1, 4);
        switch (randNum)
        {
            case 1:
                startRateOfFire = 2f;
                break;
            case 2:
                startRateOfFire = 2.25f;
                break;
            case 3:
                startRateOfFire = 2.5f;
                break;
        }

        rateOfFire = startRateOfFire;
    }

    void Update()
    {
        currentRotateSpeed = Mathf.Lerp(currentRotateSpeed, rotateSpeed, Time.deltaTime * rotateSpeed / 8);
        transform.Rotate(Vector3.forward * Time.deltaTime * currentRotateSpeed);

        if (target != null)
        {
            if (!indicateShoot && rateOfFire <= .5f)
            {
                anim.SetTrigger("StartShoot");
                indicateShoot = true;
            }

            if (rateOfFire <= 0)
            {
                Shoot();
                anim.SetTrigger("Shoot");
                indicateShoot = false;
            }
            else
            {
                rateOfFire -= Time.deltaTime;
            }
        }
    }

    void Shoot()
    {
        FindObjectOfType<AudioManager>().Play("EnemyShoot");

        var inst = Instantiate(bulletTrailPrefab, transform.position, transform.rotation);
        inst.transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 45f);

        inst = Instantiate(bulletTrailPrefab, transform.position, transform.rotation);
        inst.transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 135f);

        inst = Instantiate(bulletTrailPrefab, transform.position, transform.rotation);
        inst.transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 225f);

        inst = Instantiate(bulletTrailPrefab, transform.position, transform.rotation);
        inst.transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 315f);

        rateOfFire = startRateOfFire;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(sittingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer();
            Destroy(gameObject);
        }
    }

    public void DamageEnemy(Vector3 bulletPos, Quaternion bulletRot)
    {
        Instantiate(sittingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
        Destroy(gameObject);
    }
}
