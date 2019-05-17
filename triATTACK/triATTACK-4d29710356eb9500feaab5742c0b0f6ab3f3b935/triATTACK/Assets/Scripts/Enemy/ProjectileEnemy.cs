using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotateSpeed;

    [SerializeField] int enemyDamage;

    Transform target;
    Vector3 normDirection;

    [SerializeField] Transform deathParticlePrefab;
    [SerializeField] Transform dmgParticlePrefab;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    ScoreText scoreText;
    [SerializeField] int addScoreDeath;

    [SerializeField] float screenX;
    [SerializeField] float screenY;

    [SerializeField] int health;
   
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameObject text = GameObject.Find("ScoreText");
        scoreText = text.GetComponent<ScoreText>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
        normDirection = (target.position - transform.position).normalized;
    }

    void Update ()
    {
        transform.position += normDirection * moveSpeed * Time.deltaTime;
        transform.Rotate(Vector3.forward * rotateSpeed * Time.deltaTime);

        Vector2 newPos = transform.position;

        if (transform.position.x > screenX)
        {
            newPos.x = -screenX;
        }
        if (transform.position.x < -screenX)
        {
            newPos.x = screenX;
        }
        if (transform.position.y > screenY)
        {
            newPos.y = -screenY;
        }
        if (transform.position.y < -screenY)
        {
            newPos.y = screenY;
        }
        transform.position = newPos;
	}

    public void DamageEnemy(int damage, Vector3 bulletPos, Quaternion bulletRot)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemySpawner.spawner.KilledEnemyCounter();

            Instantiate(deathParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            scoreText.SetScore(addScoreDeath);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
        else if (health > 0)
        {
            Instantiate(dmgParticlePrefab, bulletPos, bulletRot);
            shake.Shake(shakeDuration, shakeIntensity / 4);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(deathParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer(enemyDamage);
            shake.Shake(shakeDuration * 2, shakeIntensity * 1.2f);
            Destroy(gameObject);
        }
    }

    void FindPlayer()
    {
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if (searchResult != null)
        {
            target = searchResult.transform;
        }
        else
        {
            Debug.LogError("No player found for projectile");
        }
    }
}
