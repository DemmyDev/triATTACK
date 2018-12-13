using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour {

    public float moveSpeed;

    public int enemyDamage;

    private Transform target;

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

    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    // Update is called once per frame
    void Update ()
    {
		
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
