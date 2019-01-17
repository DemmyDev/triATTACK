using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{

    public float speed;
    public int enemyDamage;

    private Transform target;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    private ScoreText scoreText;
    public int addScoreDeath;

    public Transform homingParticlePrefab;

    [System.Serializable]
    public class EnemyStats
    {
        public int health;
    }

    public EnemyStats enemyStats = new EnemyStats();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameObject text = GameObject.Find("ScoreText");
        scoreText = text.GetComponent<ScoreText>();
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
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
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