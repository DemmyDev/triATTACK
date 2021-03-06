﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RapidCollision : MonoBehaviour
{
    [SerializeField] int bulletSpeed = 28;

    [SerializeField] float rotateSpeed = 700;
    float slowDownDivider = 1.01f;
    [HideInInspector] public float slowDownSpeed;
    [HideInInspector] public Rigidbody2D rb;

    float screenX = 36.25f, screenY = 20.9f;

    GameObject player;
    Transform target;

    RapidBullet parentBul;
    [SerializeField] Animation anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        parentBul = transform.parent.GetComponent<RapidBullet>();
        slowDownSpeed = rotateSpeed;

        Vector2 direction = new Vector2(transform.right.x, transform.right.y);
        rb.velocity = direction * bulletSpeed;
        anim.Play();
    }


    void Update()
    {
        // Screen wrapping
        Vector2 pos = transform.position;

        if (pos.x > screenX) transform.position = new Vector2(-screenX, pos.y);
        if (pos.x < -screenX) transform.position = new Vector2(screenX, pos.y);
        if (pos.y > screenY) transform.position = new Vector2(pos.x, -screenY);
        if (pos.y < -screenY) transform.position = new Vector2(pos.x, screenY);
    }

    // Called every frame in RapidBullet, if it is not recalling
    public void NormalMove()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * slowDownSpeed);
        slowDownSpeed /= slowDownDivider;
    }

    // Called every frame in RapidBullet, if it is recalling
    public void RecallMove()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * 2f * Time.deltaTime);
    }

    public void PlayAnim()
    {
        anim.Stop();
        anim.Play();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            parentBul.EnemyHit(other.gameObject, transform.position, transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();

            if (playerShooting.isRecalling || playerShooting.canRecall)
            {
                AudioManager.Instance.Play("PlayerTriHit");
                parentBul.BulletRecalled(playerShooting);
                gameObject.SetActive(false);
            }
        }
    }
}
