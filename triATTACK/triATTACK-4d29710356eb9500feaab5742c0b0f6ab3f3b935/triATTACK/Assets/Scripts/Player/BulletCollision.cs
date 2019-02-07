using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreEnemyHit;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();

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

            scoreText.SetScore(5);
        }
    }
}
