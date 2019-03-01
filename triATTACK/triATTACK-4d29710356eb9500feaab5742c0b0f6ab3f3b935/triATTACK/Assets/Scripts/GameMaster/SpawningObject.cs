using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningObject : MonoBehaviour {

    [SerializeField] GameObject enemyObj;

    Animation anim;

    void Start()
    {
        anim = GetComponent<Animation>();
        anim.Play();
        Invoke("SpawnEnemy", 1f);
    }

    void SpawnEnemy()
    {
        Instantiate(enemyObj, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
