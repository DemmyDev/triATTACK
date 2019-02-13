using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;
    [SerializeField] Transform crosshair;

    float freezeFrameDuration;
    float addFreezeDuration = .025f;
    bool isFrozen;
    float pendingFreezeDuration = 0f;

    AudioSource gameSound;

    void Start()
    {
        gameSound = gameObject.GetComponent<AudioSource>();
        if (gm == null)
        {
            gm = this;
        }
        Cursor.visible = false;
    }

    void Update()
    {
        crosshair.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        crosshair.position = new Vector3(crosshair.position.x, crosshair.position.y, 10f);

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
        gameObject.GetComponent<EnemySpawner>().enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
            else if (enemy.name == "ProjectileEnemy(Clone)")
            {
                enemy.GetComponent<ProjectileEnemy>().enabled = false;
                enemy.GetComponent<PolygonCollider2D>().enabled = false;
            }
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets)
        {
            Destroy(bullet);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<Player>().enabled = false;
            player.GetComponent<PlayerShooting>().enabled = false;
        }
    }

    public void StartGame()
    {
        gameObject.GetComponent<EnemySpawner>().enabled = true;
        gameObject.GetComponent<EnemySpawner>().SpawnEnemy();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<Player>().enabled = true;
            player.GetComponent<PlayerShooting>().enabled = true;
        }
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
