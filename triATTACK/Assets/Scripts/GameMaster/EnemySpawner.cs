using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] enemy; // Enemy object
    public Transform[] spawner; // Empty GameObjects to be used for spawn locations
    public float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    private float timeBtwSpawns; // Allows time between enemy spawns to be reset
    private int randomArrayInt; // For finding the random spawn location
    private int randomEnemyInt; // For determine which enemy to spawn
    

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
        randomArrayInt = Random.Range(0, spawner.Length); // Chooses random integer for spawner array
        randomEnemyInt = Random.Range(0, enemy.Length); // Chooses random integer for enemy array
        Instantiate(enemy[randomEnemyInt], spawner[randomArrayInt].position, Quaternion.identity); // Spawns enemy object at spawner location
    }
}
