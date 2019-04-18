using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    [SerializeField] Transform destroyParticlePrefab;

    [SerializeField] int bulletSpeed;
    [SerializeField] int bulletDamage;

    void Start ()
    {
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z + Random.Range(-5f, 5f));
	}
	
	void Update ()
    {
        transform.Translate(Vector3.right * Time.deltaTime * bulletSpeed);
	}
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(destroyParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
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
            Instantiate(destroyParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}