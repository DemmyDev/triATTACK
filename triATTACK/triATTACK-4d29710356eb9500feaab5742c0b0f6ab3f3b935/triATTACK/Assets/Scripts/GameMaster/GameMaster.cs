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

        RapidBullet rapidBullet = FindObjectOfType<RapidBullet>();
        if (rapidBullet != null) Destroy(rapidBullet.gameObject);

        BounceBullet bounceBullet = FindObjectOfType<BounceBullet>();
        if (bounceBullet != null) Destroy(rapidBullet.gameObject);

        SpawningObject[] spawningObjects = FindObjectsOfType<SpawningObject>();
        foreach (SpawningObject spawningObj in spawningObjects) Destroy(spawningObj.gameObject);

        if (playerIsDead)
        {
            // Achievement checks
            TripleCheckUnlock();
            FollowCheckUnlock(scoreText.GetScore());
            RapidCheckUnlock();

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

    #region Achievements

    public void TripleCheckUnlock()
    {
        // Play 3 games to the end
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedTriple", false))
        {
            int games = ReadWriteSaveManager.Instance.GetData("GameCompletions", 0) + 1;
            ReadWriteSaveManager.Instance.SetData("GameCompletions", games, true);
             
            if (ReadWriteSaveManager.Instance.GetData("GameCompletions", 0) > 3) ReadWriteSaveManager.Instance.SetData("UnlockedTriple", true, true);
        }
    }

    public void FollowCheckUnlock(int score)
    {
        // Score of at least 20,000
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedFollow", false))
        {
            if (score >= 20000) ReadWriteSaveManager.Instance.SetData("UnlockedFollow", true, true);
        }
    }

    public void SpongeCheckUnlock(int combo)
    {
        // Get an 8-enemy combo
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedSponge", false))
        {
            if (combo >= 8) ReadWriteSaveManager.Instance.SetData("UnlockedSponge", true, true);
        }
    }

    public void RapidCheckUnlock()
    {
        // Unlock all other tris
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedRapid", false))
        {
            bool triple = ReadWriteSaveManager.Instance.GetData("UnlockedTriple", false);
            bool follow = ReadWriteSaveManager.Instance.GetData("UnlockedFollow", false);
            bool sponge = ReadWriteSaveManager.Instance.GetData("UnlockedSponge", false);
            bool bounce = ReadWriteSaveManager.Instance.GetData("UnlockedBounce", false);
            if (triple && follow && sponge && bounce) ReadWriteSaveManager.Instance.SetData("UnlockedRapid", true, true);
        }
    }

    public void BounceCheckUnlock(bool homing, bool shooting, bool sitting, bool dashing)
    {
        // Defeat each enemy type in one shot
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedBounce", false))
        {
            if (homing && shooting && sitting && dashing) ReadWriteSaveManager.Instance.SetData("UnlockedBounce", true, true);
        }
    }

    #endregion
}