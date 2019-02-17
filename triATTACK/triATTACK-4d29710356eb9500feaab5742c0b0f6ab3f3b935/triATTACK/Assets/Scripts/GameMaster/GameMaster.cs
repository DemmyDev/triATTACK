using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    float freezeFrameDuration;
    float addFreezeDuration = .025f;
    bool isFrozen;
    float pendingFreezeDuration = 0f;

    EnemySpawner spawner;
    AudioSource gameSound;

    void Start()
    {
        spawner = gameObject.GetComponent<EnemySpawner>();
        gameSound = gameObject.GetComponent<AudioSource>();
        if (gm == null)
        {
            gm = this;
        }
    }

    void Update()
    {
        if (pendingFreezeDuration > 0 && !isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
    }

    public void Freeze(float duration)
    {
        freezeFrameDuration = duration;
        pendingFreezeDuration = freezeFrameDuration;
    }

    IEnumerator DoFreeze()
    {
        isFrozen = true;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(freezeFrameDuration);

        Time.timeScale = 1f;
        pendingFreezeDuration = 0f;
        isFrozen = false;
    }

    public void PlaySound(AudioSource sound)
    {
        gameSound.clip = sound.clip;
        gameSound.pitch = sound.pitch;
        sound.Play();
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void DisableObjectScripts()
    {
        spawner.enabled = false;

        /*GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (enemy.name == "HomingEnemy(Clone)")
            {
                enemy.GetComponent<HomingEnemy>().enabled = false;
                enemy.GetComponent<CapsuleCollider2D>().enabled = false;
            }
            else if(enemy.name == "ShootingEnemy(Clone)")
            {
                enemy.GetComponent<ShootingEnemy>().enabled = false;
                enemy.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }*/
    }

    public void StartGame()
    {
        spawner.enabled = true;
        spawner.SpawnEnemy();
    }


    public void DeleteObjectsOnPlayerDeath()
    {
        gameObject.GetComponent<EnemySpawner>().enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            GameObject.Destroy(bullet);
        }
    }
}
