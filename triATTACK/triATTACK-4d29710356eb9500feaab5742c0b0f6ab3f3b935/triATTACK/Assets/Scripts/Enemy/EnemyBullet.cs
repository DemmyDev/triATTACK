using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {

    public Transform destroyParticlePrefab;

    public int bulletSpeed;
    public int bulletDamage;

    void Start ()
    {
        gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z + Random.Range(-8f, 8f));
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
    }
}