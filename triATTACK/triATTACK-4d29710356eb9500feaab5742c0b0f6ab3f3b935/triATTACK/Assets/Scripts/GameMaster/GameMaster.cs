using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    [SerializeField] Transform player;
    [SerializeField] int health;
    HealthUI healthUI;
    DeathText deathText;
    ScoreText scoreText;
    float freezeFrameDuration;
    bool isFrozen;
    float pendingFreezeDuration = 0f;
    public static bool isFreezing = false;

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
        isFreezing = true;

        yield return new WaitForSecondsRealtime(freezeFrameDuration);

        Time.timeScale = 1f;
        pendingFreezeDuration = 0f;
        isFrozen = false;
        isFreezing = false;
    }

    public void RestartScene()
    {
        LoadingManager.Instance.LoadScene("tri.Attack", gameObject.scene);
    }

    public void DisableObjectScripts()
    {
        EnemySpawner.spawner.SetIsSpawning(false);

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
        EnemySpawner.spawner.SetIsSpawning(true);
        EnemySpawner.spawner.SpawnEnemy();
    }

    void DeleteObjectsOnPlayerDamage(bool playerIsDead)
    {
        EnemySpawner.spawner.SetIsSpawning(false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) Destroy(enemy);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) Destroy(bullet);

        Bullet triBullet = FindObjectOfType<Bullet>();
        if (triBullet != null) Destroy(triBullet.gameObject);

        TripleBullet tripleBullet = FindObjectOfType<TripleBullet>();
        if (tripleBullet != null) Destroy(tripleBullet.gameObject);

        FollowBullet followBullet = FindObjectOfType<FollowBullet>();
        if (followBullet != null) Destroy(followBullet.gameObject);

        SpongeBullet spongeBullet = FindObjectOfType<SpongeBullet>();
        if (spongeBullet != null) Destroy(spongeBullet.gameObject);

        SpawningObject[] spawningObjects = FindObjectsOfType<SpawningObject>();
        foreach (SpawningObject spawningObj in spawningObjects) Destroy(spawningObj.gameObject);

        if (playerIsDead)
        {
            healthUI.DisableUI();
            deathText.EnableText();
            scoreText.MoveText();
            scoreText.SetHighScore();
        }
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

    public void SetHealthUI(HealthUI UI)
    {
        healthUI = UI;
    }

    public void SetDeathText(DeathText text)
    {
        deathText = text;
    }

    public void SetScoreText(ScoreText text)
    {
        scoreText = text;
    }

    public void Respawn(GameObject playerObj)
    {
        health -= 1;
        healthUI.SetHealthSprites(health);

        if (health <= 0)
        {
            DeleteObjectsOnPlayerDamage(true);
            Player.SetDeathBool(true);
            Destroy(playerObj);
        }
        else StartCoroutine(RespawnPlayer(playerObj));
    }

    IEnumerator RespawnPlayer(GameObject playerObj)
    {
        DeleteObjectsOnPlayerDamage(false);
        Destroy(playerObj);

        yield return new WaitForSeconds(1f);

        Instantiate(player.gameObject, Vector3.zero, Quaternion.identity);
        EnemySpawner.spawner.SetIsSpawning(true);
    }

    public void ResetHealth()
    {
        health = 3;
    }
}