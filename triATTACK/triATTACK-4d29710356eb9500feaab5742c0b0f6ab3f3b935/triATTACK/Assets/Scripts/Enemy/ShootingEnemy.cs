﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;
    int speed;
    [SerializeField] float stoppingDistance;
    [SerializeField] float retreatDistance;

    Transform target;
    float startRateOfFire, rateOfFire;

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
        speed = Random.Range(minSpeed, maxSpeed + 1);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        int randNum = Random.Range(1, 4);
        switch(randNum)
        {
            case 1:
                startRateOfFire = 1f;
                break;
            case 2:
                startRateOfFire = 1.25f;
                break;
            case 3:
                startRateOfFire = 1.5f;
                break;
        }

        rateOfFire = startRateOfFire;

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
        Invoke("Shoot", .25f);
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
        Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
        rateOfFire = startRateOfFire;
    }

    public void DamageEnemy(int damage, Vector3 bulletPos, Quaternion bulletRot)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemySpawner spawner = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<EnemySpawner>();
            spawner.KilledEnemyCounter();

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
