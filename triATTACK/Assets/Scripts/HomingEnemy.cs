using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{

    public float speed;
    public int enemyDamage;

    private Transform target;
    private float nextTimeToSearch = 0;

    [Range(0f, 2f)]
    public float shakeIntensity;
    private ScreenShake shake;
    public float shakeDuration;

    [System.Serializable]
    public class EnemyStats
    {
        public int health;
    }

    public EnemyStats enemyStats = new EnemyStats();

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        
        if (target == null)
        {
            FindPlayer();
            return;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        }        
    }

    public void DamageEnemy(int damage)
    {
        enemyStats.health -= damage;
        if (enemyStats.health <= 0)
        {
            GameMaster.KillHomingEnemy(this);
            shake.Shake(shakeDuration, shakeIntensity);
        }
    }

    void FindPlayer()
    {
        if (nextTimeToSearch <= Time.time)
        {
            GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
            if (searchResult != null)
            {
                target = searchResult.transform;
            }
            nextTimeToSearch = Time.time + 0.5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer(enemyDamage);
        }
    }
}
