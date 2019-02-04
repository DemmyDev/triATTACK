using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int bulletSpeed;
    public int bulletDamage;

    private ScoreText scoreText;
    public int addScoreEnemyHit;


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

            Destroy(gameObject);
        }
        else if (other.CompareTag("ObjectDestroy"))
        {
            Destroy(gameObject);
        }
    }
}