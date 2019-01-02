using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour {

    public float moveSpeed;
    public float rotateSpeed;

    public int enemyDamage;

    private Transform target;
    private Vector3 normDirection;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    private ScoreText scoreText;
    public int addScoreDeath;

    public float screenX;
    public float screenY;

    [System.Serializable]
    public class EnemyStats
    {
        public int health;
    }

    public EnemyStats enemyStats = new EnemyStats();

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

    public void DamageEnemy(int damage)
    {
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
        {
            scoreText.SetScore(addScoreDeath);
            Destroy(gameObject);
            shake.Shake(shakeDuration, shakeIntensity);
        }
        else if (enemyStats.health > 0)
        {
            shake.Shake(shakeDuration, shakeIntensity / 4);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
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
