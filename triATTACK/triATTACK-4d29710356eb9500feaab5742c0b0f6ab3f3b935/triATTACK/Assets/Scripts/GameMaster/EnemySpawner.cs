using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    [SerializeField] GameObject[] enemy; // Enemy object
    [SerializeField] Transform[] spawner; // Empty GameObjects to be used for spawn locations
    [SerializeField] float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    float timeBtwSpawns; // Allows time between enemy spawns to be reset
    [SerializeField] float subtractTime;
    int enemyInt;
    int enemiesKilled = 0;

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
        int arrayInt = Random.Range(0, spawner.Length); // Chooses random integer for spawner array
        int randomInt = Random.Range(0, 100); // Chooses random integer for enemy array
        if (randomInt < 50)
        {
            enemyInt = 0;
            //Debug.Log("Spawn homing enemy");
        }
        else if (randomInt < 91 && randomInt >= 50)
        {
            enemyInt = 1;
            //Debug.Log("Spawn shooting enemy");
        }
        else
        {
            enemyInt = 2;
            //Debug.Log("Spawn projectile enemy");
        }
        Instantiate(enemy[enemyInt], spawner[arrayInt].position, Quaternion.identity); // Spawns enemy object at spawner location
    }

    public void KilledEnemyCounter()
    {
        enemiesKilled++;
        //Debug.Log("Enemies Killed: " + enemiesKilled);

        
        if (enemiesKilled == 34 || enemiesKilled == 25 || enemiesKilled == 18 || enemiesKilled == 12 || enemiesKilled == 8 || enemiesKilled == 4)
        {
            startTimeBtwSpawns -= subtractTime;
            //Debug.Log("New spawn timer: " + startTimeBtwSpawns);
        }
        
    }
}
