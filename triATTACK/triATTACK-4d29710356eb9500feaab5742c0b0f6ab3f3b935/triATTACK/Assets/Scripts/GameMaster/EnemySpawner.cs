using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner spawner;

    [SerializeField] GameObject[] enemy; // Enemy object
    [SerializeField] float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    float timeBtwSpawns; // Allows time between enemy spawns to be reset
    [SerializeField] float subtractTime;
    int enemiesKilled = 0;

    void Start()
    {
        if (spawner == null) spawner = this;
        GameMaster.Instance.DisableObjectScripts();
        timeBtwSpawns = startTimeBtwSpawns; // Set time between spawns to adjustable variable
    }

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
        if (randomInt > 70 && enemiesKilled > 12)
        {
            // Sitting enemy
            Vector2 spawnPos = new Vector2(Random.Range(-20f, 20f), Random.Range(-10f, 10f));
            var inst = Instantiate(enemy[1], spawnPos, Quaternion.identity);
            inst.transform.eulerAngles = new Vector3(0f, 0f, 45f);
        }
        else if (randomInt > 40 && enemiesKilled > 6)
        {
            // Shooting enemy
            Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
            Instantiate(enemy[0], spawnPos, Quaternion.identity);
        }
        else
        {
            Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
            // Homing enemy
            Instantiate(enemy[2], spawnPos, Quaternion.identity);
        }
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
