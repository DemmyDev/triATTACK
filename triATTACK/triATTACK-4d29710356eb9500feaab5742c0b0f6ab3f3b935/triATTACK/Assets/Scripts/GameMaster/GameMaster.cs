using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    float freezeFrameDuration;
    bool isFrozen;
    float pendingFreezeDuration = 0f;

    EnemySpawner spawner;

    void Start()
    {
        spawner = gameObject.GetComponent<EnemySpawner>();
        DisableObjectScripts();
        if (gm == null) gm = this;
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
        Debug.Log("Do freeze");

        yield return new WaitForSecondsRealtime(freezeFrameDuration);

        Time.timeScale = 1f;
        pendingFreezeDuration = 0f;
        isFrozen = false;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(1);
    }

    public void DisableObjectScripts()
    {
        spawner.enabled = false;

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
                enemy.GetComponent<PolygonCollider2D>().enabled = false;
            }
            else if (enemy.name == "SittingEnemy(Clone)")
            {
                enemy.GetComponent<SittingEnemy>().enabled = false;
                enemy.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);
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
        foreach (GameObject enemy in enemies) Destroy(enemy);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);

        Bullet triBullet = FindObjectOfType<Bullet>();
        if (triBullet != null) Destroy(triBullet.gameObject);

        TripleBullet tripleBullet = FindObjectOfType<TripleBullet>();
        if (tripleBullet != null) Destroy(tripleBullet.gameObject);

        SpawningObject[] spawners = FindObjectsOfType<SpawningObject>();
        foreach (SpawningObject spawner in spawners) Destroy(spawner.gameObject);
    }

    public void ChangeBackColor(float colorNum)
    {
        StartCoroutine(CamBackgroundColorChange(colorNum));
    }

    IEnumerator CamBackgroundColorChange(float colorNum)
    {
        Camera cam = Camera.main;
        cam.backgroundColor = new Color(colorNum, colorNum, colorNum);
        yield return new WaitForSecondsRealtime(.075f);
        cam.backgroundColor = Color.black;
    }
}