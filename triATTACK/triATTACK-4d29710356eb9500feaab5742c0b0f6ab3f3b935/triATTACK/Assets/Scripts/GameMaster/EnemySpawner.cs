using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public static EnemySpawner spawner;

    [SerializeField] GameObject[] enemy; // Enemy object
    [SerializeField] float startTimeBtwSpawns; // Adjustable variable for the time between enemy spawns
    float timeBtwSpawns; // Allows time between enemy spawns to be reset
    [SerializeField] float shootingEnemySpawnTime;
    [SerializeField] float sittingEnemySpawnTime;
    [SerializeField] float dashingEnemySpawnTime;
    float playTime;
    bool isSpawning = false;

    void Start()
    {
        if (spawner == null) spawner = this;
        GameMaster.Instance.DisableObjectScripts();
        timeBtwSpawns = startTimeBtwSpawns; // Set time between spawns to adjustable variable
    }

    void Update()
    {
        if (isSpawning)
        {
            playTime += Time.deltaTime;

            if (timeBtwSpawns <= 0) SpawnEnemy();
            else timeBtwSpawns -= Time.deltaTime;
        }
    }

    public void SpawnEnemy()
    {
        // Chooses random int in array, limited based on play time
        int randomInt = 0;
        if (playTime > sittingEnemySpawnTime) randomInt = Random.Range(1, 101); // Homing, Shooting, Dashing, or Sitting
        else if (playTime > dashingEnemySpawnTime) randomInt = Random.Range(1, 86); // Homing, Shooting, or Dashing
        else if (playTime > shootingEnemySpawnTime) randomInt = Random.Range(1, 71); // Homing or Shooting
        else randomInt = Random.Range(1, 2); // Homing guaranteed

        if (randomInt > 85 && playTime > sittingEnemySpawnTime)
        {            
            // Sitting enemy
            Vector2 spawnPos = new Vector2(Random.Range(-20f, 20f), Random.Range(-10f, 10f));
            var inst = Instantiate(enemy[1], spawnPos, Quaternion.identity);
            inst.transform.eulerAngles = new Vector3(0f, 0f, 45f);
        }
        else if (randomInt > 70 && playTime > dashingEnemySpawnTime)
        {
            // Dashing enemy
            Vector2 spawnPos = new Vector2(Random.Range(-34f, 34f), Random.Range(-18.5f, 18.5f));
            Instantiate(enemy[3], spawnPos, Quaternion.identity);
        }
        else if (randomInt > 35 && playTime > shootingEnemySpawnTime)
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

        // Check spawn rates
        if (playTime < 12) startTimeBtwSpawns = 2f;
        else if (playTime < 24) startTimeBtwSpawns = 1.75f;
        else if (playTime < 36) startTimeBtwSpawns = 1.5f;
        else if (playTime < 50) startTimeBtwSpawns = 1.25f;
        else if (playTime < 75) startTimeBtwSpawns = 1f;
        else startTimeBtwSpawns = .75f;
        
        timeBtwSpawns = startTimeBtwSpawns;
    }

    public void SetIsSpawning(bool spawning)
    {
        isSpawning = spawning;
    }
}
