using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour {
    
    public float speed;
    public float stoppingDistance;
    public float retreatDistance;

    private Transform target;

    private float timeBtwShots;
    public float startTimeBtwShots;
    public GameObject bulletTrailPrefab;
    private Transform firePoint;

    public Transform deathParticlePrefab;
    public Transform dmgParticlePrefab;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    private ScoreText scoreText;
    public int addScoreDeath;

    [System.Serializable]
    public class EnemyStats
    {
        public int health;
    }

    public EnemyStats enemyStats = new EnemyStats();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameObject text = GameObject.Find("ScoreText");
        scoreText = text.GetComponent<ScoreText>();
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
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
        {
            Instantiate(deathParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            scoreText.SetScore(addScoreDeath);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
        else if (enemyStats.health > 0)
        {
            Instantiate(dmgParticlePrefab, bulletPos, bulletRot);
            shake.Shake(shakeDuration, shakeIntensity / 4);
        }
    }
}
