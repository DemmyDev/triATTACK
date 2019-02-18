using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollisionMainMenu : MonoBehaviour
{
    Bullet bullet;

    void Start()
    {
        bullet = gameObject.transform.parent.GetComponent<Bullet>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            FindObjectOfType<AudioManager>().Play("BulletWallHit");
            bullet.slowDownSpeed = 0f;
            bullet.rb.velocity = Vector2.zero;
        }
    }
}
