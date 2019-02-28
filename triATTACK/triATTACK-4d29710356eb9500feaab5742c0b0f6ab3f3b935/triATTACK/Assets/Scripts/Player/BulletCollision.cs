using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    int addScoreStacking = 0;
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
                scoreText.SetScore(addScoreHomingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreHomingHit + addScoreStacking, homingEnemy.transform.position);
                homingEnemy.DamageEnemy(bulletDamage);
            }
            else if (shootingEnemy)
            {
                scoreText.SetScore(addScoreShootingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreShootingHit + addScoreStacking, shootingEnemy.transform.position);
                shootingEnemy.DamageEnemy(bulletDamage, bulletPos, bulletRot);
            }

            addScoreStacking += 100;
        }

        if (other.CompareTag("Wall"))
        {
            if (!Bullet.isRecalling)
            {
                FindObjectOfType<AudioManager>().Play("BulletWallHit");
            }
            bullet.slowDownSpeed = 0f;
            bullet.rb.velocity = Vector2.zero;
        }
    }
}
