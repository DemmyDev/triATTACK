using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] enemy; // Enemy object
    public Transform[] spawner; // Empty GameObjects to be used for spawn locations
    public float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    private float timeBtwSpawns; // Allows time between enemy spawns to be reset
    private int enemyInt;
    

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
        int randomInt = Random.Range(1, 100); // Chooses random integer for enemy array
        if (randomInt < 36)
        {
            enemyInt = 0;
            Debug.Log("Spawn homing enemy");
        }
        else if (randomInt < 75 && randomInt > 35)
        {
            enemyInt = 1;
            Debug.Log("Spawn shooting enemy");
        }
        else
        {
            enemyInt = 2;
            Debug.Log("Spawn projectile enemy");
        }
        Instantiate(enemy[enemyInt], spawner[arrayInt].position, Quaternion.identity); // Spawns enemy object at spawner location
    }
}
