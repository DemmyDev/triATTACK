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

    // On start
	void Start ()
    {
        timeBtwSpawns = startTimeBtwSpawns; // Set time between spawns to adjustable variable
	}
	
	// Each frame
	void Update ()
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
        if (randomInt > 60 && shootingEnemyTotal < 5)
        {
            enemyInt = 0; // Shooting enemy
            shootingEnemyTotal++;
        }
        else
        {
            enemyInt = 1; // Homing enemy
        }

        Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
        Instantiate(enemy[enemyInt], spawnPos, Quaternion.identity);
    }

    public void SubtractShootingEnemyCounter()
    {
        shootingEnemyTotal--;
    }

    public void KilledEnemyCounter()
    {
        enemiesKilled++;
        
        if (enemiesKilled == 34 || enemiesKilled == 25 || enemiesKilled == 18 || enemiesKilled == 12 || enemiesKilled == 8 || enemiesKilled == 4)
        {
            startTimeBtwSpawns -= subtractTime;
        }
    }
}
