﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] int bulletDamage;

    ScoreText scoreText;
    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    [SerializeField] int addScoreSittingHit;
    int addScoreStacking = 0;
    [SerializeField] Transform flashObj;
    [SerializeField] float startFreezeDuration;
    float freezeDuration;
    [SerializeField] float addFreezeDuration;
    float colorNum = .15f;

    public float pitch = .3f;

    bool isShaking = false;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
        freezeDuration = startFreezeDuration;
        Invoke("ObjectShake", 2f);
    }

    void ObjectShake()
    {
        isShaking = true;
    }

    void Update()
    {
        if (isShaking && !Bullet.isRecalling)
        {
            transform.localPosition = new Vector2(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));
        }
        else if (Bullet.isRecalling)
        {
            transform.localPosition = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.gm.ChangeBackColor(colorNum);
            if (colorNum < .35f)
            {
                colorNum += .05f;
            }

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
            SittingEnemy sittingEnemy = other.gameObject.GetComponent<SittingEnemy>();

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
            else if (sittingEnemy)
            {
                scoreText.SetScore(addScoreSittingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreSittingHit + addScoreStacking, sittingEnemy.transform.position);
                sittingEnemy.DamageEnemy(bulletDamage, bulletPos, bulletRot);
            }

            addScoreStacking += 100;
        }
    }
}