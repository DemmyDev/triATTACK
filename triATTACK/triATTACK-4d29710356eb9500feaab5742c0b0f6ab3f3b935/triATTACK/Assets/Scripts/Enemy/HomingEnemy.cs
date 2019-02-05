using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] int enemyDamage;
    [SerializeField] int health;

    Transform target;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    ScoreText scoreText;
    [SerializeField] int addScoreDeath;

    [SerializeField] Transform homingParticlePrefab;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }        
    }

    public void DamageEnemy(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EnemySpawner spawner = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<EnemySpawner>();
            spawner.KilledEnemyCounter();

            Instantiate(homingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            scoreText.SetScore(addScoreDeath);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(homingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer(enemyDamage);
            Destroy(gameObject);
        }
    }
}