using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour
{
    [SerializeField] int minSpeed;
    [SerializeField] int maxSpeed;
    float currentSpeed = 0;
    int speed;
    [SerializeField] int enemyDamage;
    [SerializeField] int health;

    Transform target;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] Transform homingParticlePrefab;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed + 1);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        shake = Camera.main.GetComponent<ScreenShake>();
        if (shake == null)
        {
            Debug.LogError("No camera found for screenshake");
        }
    }

    void Update()
    {
        if (target != null)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * speed / 2);
            transform.position = Vector2.MoveTowards(transform.position, target.position, currentSpeed * Time.deltaTime);
        }        
    }

    public void DamageEnemy()
    {
        Instantiate(homingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(gameObject);
        shake.Shake(shakeDuration, shakeIntensity);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(homingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer();
            Destroy(gameObject);
        }
    }
}