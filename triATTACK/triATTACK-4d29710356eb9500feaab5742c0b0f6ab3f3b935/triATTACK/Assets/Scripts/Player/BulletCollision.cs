using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreEnemyHit;
    [SerializeField] Transform flashObj;
    [SerializeField] float startFreezeDuration;
    float freezeDuration;
    [SerializeField] float addFreezeDuration;

    public float pitch = .3f;

    Bullet bullet;

    void Start()
    {
        bullet = gameObject.transform.parent.GetComponent<Bullet>();
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
        freezeDuration = startFreezeDuration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.gm.Freeze(freezeDuration);
            if (freezeDuration < .15f)
            {
                freezeDuration += addFreezeDuration;
            }

            // Make the flash object animate
            var flashInst = Instantiate(flashObj, other.transform.position, Quaternion.identity);
            Destroy(flashInst.gameObject, .25f);

            FindObjectOfType<AudioManager>().Play("EnemyHit");
            pitch += .05f;

            HomingEnemy homingEnemy = other.gameObject.GetComponent<HomingEnemy>();
            ShootingEnemy shootingEnemy = other.gameObject.GetComponent<ShootingEnemy>();

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
            
            scoreText.SetScore(5);
        }

        if (other.CompareTag("Wall"))
        {
            bullet.slowDownSpeed = 0f;
            bullet.rb.velocity = Vector2.zero;
        }
    }
}
