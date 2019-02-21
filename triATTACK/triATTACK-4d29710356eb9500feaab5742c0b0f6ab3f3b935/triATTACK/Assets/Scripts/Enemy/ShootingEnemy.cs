using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float retreatDistance;

    Transform target;

    float timeBtwShots;
    [SerializeField] float startTimeBtwShots;
    [SerializeField] GameObject bulletTrailPrefab;
    Transform firePoint;

    [SerializeField] Transform deathParticlePrefab;
    [SerializeField] Transform dmgParticlePrefab;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] int health;
    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timeBtwShots = startTimeBtwShots;

        firePoint = transform.Find("BulletSpawn");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint found");
        }
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, target.position) < retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
            }

            if (timeBtwShots <= 0)
            {
                FindObjectOfType<AudioManager>().Play("EnemyShoot");
                Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
                timeBtwShots = startTimeBtwShots;
            }
            else
            {
                timeBtwShots -= Time.deltaTime;
            }
        }
    }

    public void DamageEnemy(int damage, Vector3 bulletPos, Quaternion bulletRot)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemySpawner spawner = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<EnemySpawner>();
            spawner.KilledEnemyCounter();
            spawner.SubtractShootingEnemyCounter();

            Instantiate(deathParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
        else if (health > 0)
        {
            Instantiate(dmgParticlePrefab, bulletPos, bulletRot);
            shake.Shake(shakeDuration, shakeIntensity / 4);
        }
    }
}
