using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SittingEnemyBullet : MonoBehaviour
{

    [SerializeField] Transform destroyParticlePrefab;

    [SerializeField] int bulletSpeed;
    [SerializeField] int bulletDamage;

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(destroyParticlePrefab, transform.position, transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer(bulletDamage);
            Destroy(gameObject);
        }

        if (other.CompareTag("ObjectDestroy"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("TriBullet"))
        {
            AudioManager.Instance.Play("HitEnemyBullet");
            Instantiate(destroyParticlePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if (other.GetComponent<TripleCollision>() != null)
        {
            AudioManager.Instance.Play("HitEnemyBullet");
            Instantiate(destroyParticlePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}