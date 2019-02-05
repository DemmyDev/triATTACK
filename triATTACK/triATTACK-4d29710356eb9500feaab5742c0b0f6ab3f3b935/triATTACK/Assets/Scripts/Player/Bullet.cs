using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreEnemyHit;

    public static bool isRecalling;
    Transform target;

    void Start()
    {
        isRecalling = false;
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        if (!isRecalling)
        {
            // This line needs rewriting to allow constant bullet rotation
            transform.Translate(Vector3.up * Time.deltaTime * bulletSpeed);
        }
        else if (isRecalling)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HomingEnemy homingEnemy = other.gameObject.GetComponent<HomingEnemy>();
            ShootingEnemy shootingEnemy = other.gameObject.GetComponent<ShootingEnemy>();
            ProjectileEnemy projectileEnemy = other.gameObject.GetComponent<ProjectileEnemy>();

            Vector3 bulletPos = gameObject.transform.position;
            Quaternion bulletRot = gameObject.transform.rotation;

            if (homingEnemy)
            {
                homingEnemy.DamageEnemy(bulletDamage);
            }
            else if (shootingEnemy)
            {
                shootingEnemy.DamageEnemy(bulletDamage, bulletPos, bulletRot);                
            }
            else if (projectileEnemy)
            {
                projectileEnemy.DamageEnemy(bulletDamage, bulletPos, bulletRot);
            }

            scoreText.SetScore(addScoreEnemyHit);
        }
    }
}