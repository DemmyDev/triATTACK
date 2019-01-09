using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int bulletDamage;

    private ScoreText scoreText;
    public int addScoreEnemyHit;

    public Transform projectileDmgParticlePrefab;
    public Transform shootingDmgParticlePrefab;
    public Transform homingDeathParticlePrefab;
    public Transform projectileDeathParticlePrefab;
    public Transform shootingDeathParticlePrefab;

    void Start()
    {
        GameObject text = GameObject.Find("ScoreText");
        scoreText = text.GetComponent<ScoreText>();
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HomingEnemy homingEnemy = other.gameObject.GetComponent<HomingEnemy>();
            ShootingEnemy shootingEnemy = other.gameObject.GetComponent<ShootingEnemy>();
            ProjectileEnemy projectileEnemy = other.gameObject.GetComponent<ProjectileEnemy>();

            if (homingEnemy)
            {
                homingEnemy.DamageEnemy(bulletDamage);
                Instantiate(homingDeathParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            }
            else if (shootingEnemy)
            {
                shootingEnemy.DamageEnemy(bulletDamage);
                Instantiate(shootingDmgParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
                
            }
            else if (projectileEnemy)
            {
                projectileEnemy.DamageEnemy(bulletDamage);
                Instantiate(projectileDmgParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            }

            scoreText.SetScore(addScoreEnemyHit);

            Destroy(gameObject);
        }
        else if (other.CompareTag("ObjectDestroy"))
        {
            Destroy(gameObject);
        }
    }
}