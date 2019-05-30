using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    [SerializeField] Transform destroyParticlePrefab;

    [SerializeField] int bulletSpeed;
    [SerializeField] int bulletDamage;

    void Start ()
    {
        gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + Random.Range(-5f, 5f));
	}
	
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
	}
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(destroyParticlePrefab, transform.position, transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer();
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