using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour {
    
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private Transform target;
    private float nextTimeToSearch = 0;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject bulletTrailPrefab;
    private Transform firePoint;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    [System.Serializable]
    public class EnemyStats
    {
        public int health;
    }

    public EnemyStats enemyStats = new EnemyStats();

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

        if (target == null)
        {
            FindPlayer();
            return;
        }

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
            Instantiate(bulletTrailPrefab, firePoint.position, firePoint.rotation);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }
    }

    public void DamageEnemy(int damage)
    {
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
        {
            GameMaster.KillShootingEnemy(this);
            shake.Shake(shakeDuration, shakeIntensity);
        }
    }

    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
            {
                target = searchResult.transform;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }
}
