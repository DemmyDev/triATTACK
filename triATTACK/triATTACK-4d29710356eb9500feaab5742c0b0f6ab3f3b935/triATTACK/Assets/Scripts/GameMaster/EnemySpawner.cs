using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject[] enemy; // Enemy object
    [SerializeField] float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    float timeBtwSpawns; // Allows time between enemy spawns to be reset
    [SerializeField] float subtractTime;
    int enemyInt;
    int enemiesKilled = 0;
    public int shootingEnemyTotal;
    public int sittingEnemyTotal;

    // On start
    void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns; // Set time between spawns to adjustable variable
    }

    // Each frame
    void Update()
    {
        if (timeBtwSpawns <= 0)
        {
            SpawnEnemy();
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }

    public void SpawnEnemy()
    {
        int randomInt = Random.Range(0, 100); // Chooses random integer for enemy array
        if (randomInt > 70 && shootingEnemyTotal < 5)
        {
            // Shooting enemy
            Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
            shootingEnemyTotal++;
            Instantiate(enemy[0], spawnPos, Quaternion.identity);
        }
        else if (randomInt > 40 && sittingEnemyTotal < 2)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-20f, 20f), Random.Range(-10f, 10f));
            enemyInt = 1; // Sitting enemy
            sittingEnemyTotal++;
            var inst = Instantiate(enemy[1], spawnPos, Quaternion.identity);
            inst.transform.eulerAngles = new Vector3(0f, 0f, 45f);
        }
        else
        {
            Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
            enemyInt = 2; // Homing enemy
            Instantiate(enemy[2], spawnPos, Quaternion.identity);
        }
    }

    public void SubtractShootingEnemyCounter()
    {
        shootingEnemyTotal--;
    }

    public void SubtractSittingEnemyCounter()
    {
        sittingEnemyTotal--;
    }

    public void KilledEnemyCounter()
    {
        enemiesKilled++;
        
        if (enemiesKilled == 40 || enemiesKilled == 28 || enemiesKilled == 20 || enemiesKilled == 12 || enemiesKilled == 6 || enemiesKilled == 3)
        {
            startTimeBtwSpawns -= subtractTime;
        }
    }
}
