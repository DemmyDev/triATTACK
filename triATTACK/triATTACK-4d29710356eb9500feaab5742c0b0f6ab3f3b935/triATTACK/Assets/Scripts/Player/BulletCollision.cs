using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreEnemyHit;

    AudioSource enemyHitSound;

    void Start()
    {
        enemyHitSound = gameObject.GetComponent<AudioSource>();
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.gm.Freeze();

            GameMaster.gm.PlaySound(enemyHitSound);
            enemyHitSound.pitch += .05f;

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
