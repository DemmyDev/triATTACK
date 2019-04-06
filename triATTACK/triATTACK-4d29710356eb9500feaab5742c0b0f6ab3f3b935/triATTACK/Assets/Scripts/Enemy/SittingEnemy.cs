using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingEnemy : MonoBehaviour
{
    [SerializeField] float minRotateSpeed;
    [SerializeField] float maxRotateSpeed;
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


    void Start()
    {
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
                startRateOfFire = 1.5f;
                break;
            case 2:
                startRateOfFire = 1.75f;
                break;
            case 3:
                startRateOfFire = 2f;
                break;
        }

        rateOfFire = startRateOfFire;

        Invoke("Shoot", .5f);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);

        if (target != null)
        {
            if (rateOfFire <= 0)
            {
                Shoot();
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
            player.DamagePlayer(1);
            Destroy(gameObject);
        }
    }

    public void DamageEnemy(int damage, Vector3 bulletPos, Quaternion bulletRot)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemySpawner spawner = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<EnemySpawner>();
            spawner.KilledEnemyCounter();
            spawner.SubtractSittingEnemyCounter();

            Instantiate(sittingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
    }
}
