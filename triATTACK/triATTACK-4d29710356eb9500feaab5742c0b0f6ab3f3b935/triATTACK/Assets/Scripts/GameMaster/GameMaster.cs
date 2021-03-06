﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Steamworks;

public class GameMaster : Singleton<GameMaster>
{
    [SerializeField] Transform player;
    [SerializeField] int health;
    [SerializeField] bool steamBuild;
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
        AudioManager.Instance.Stop("MusicGame");
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
            else if (enemy.name == "ShootingEnemy(Clone)")
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

        if (playerIsDead)
        {
            AudioManager.Instance.Stop("MusicGame");
            // Achievement checks
            TripleCheckUnlock();
            FollowCheckUnlock(scoreText.GetScore());
            RapidCheckUnlock();

            healthUI.DisableUI();
            deathText.EnableText();
            scoreText.MoveText();
            scoreText.SetHighScore();
            ReadWriteSaveManager.Instance.SetData("PlayedOnce", true, true);
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) if (enemy != null) Destroy(enemy);

        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");
        foreach (GameObject bullet in bullets) if (bullet != null) Destroy(bullet);

        SpawningObject[] spawningObjects = FindObjectsOfType<SpawningObject>();
        foreach (SpawningObject spawningObj in spawningObjects) if (spawningObj != null) Destroy(spawningObj.gameObject);

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
        if (bounceBullet != null) Destroy(bounceBullet.gameObject);
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
        int setGames = ReadWriteSaveManager.Instance.GetData("GameCompletions", 0) + 1;
        ReadWriteSaveManager.Instance.SetData("GameCompletions", setGames, true);
        int games = ReadWriteSaveManager.Instance.GetData("GameCompletions", 0);

        // Play 3 games to the end
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedTriple", false))
        {
            if (games > 3)
            {
                ReadWriteSaveManager.Instance.SetData("UnlockedTriple", true, true);
                if (steamBuild) SteamAchievements.UnlockAchievement("TRIPLE_UNLOCK");
            }
        }

        // Play 50 games to the end
        if (games > 50 && steamBuild) SteamAchievements.UnlockAchievement("TRI_ADDICT");

        // Play 100 games to the end
        if (games > 100 && steamBuild) SteamAchievements.UnlockAchievement("TRI_FOREVER");
    }

    public void FollowCheckUnlock(int score)
    {
        // Score of at least 20,000
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedFollow", false))
        {
            if (score >= 20000)
            {
                ReadWriteSaveManager.Instance.SetData("UnlockedFollow", true, true);
                if (steamBuild) SteamAchievements.UnlockAchievement("FOLLOW_UNLOCK");
            }
        }

        // Score of at least 50,000
        if (score >= 50000 && steamBuild) SteamAchievements.UnlockAchievement("TRI_MASTER");

        // Score of at least 100,000
        if (score >= 100000 && steamBuild) SteamAchievements.UnlockAchievement("TRI_LEGEND");
    }

    public void SpongeCheckUnlock(int combo)
    {
        // Get an 8-enemy combo
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedSponge", false))
        {
            if (combo >= 6)
            {
                ReadWriteSaveManager.Instance.SetData("UnlockedSponge", true, true);
                if (steamBuild) SteamAchievements.UnlockAchievement("SPONGE_UNLOCK");
            }
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
            if (triple && follow && sponge && bounce)
            {
                ReadWriteSaveManager.Instance.SetData("UnlockedRapid", true, true);
                if (steamBuild) SteamAchievements.UnlockAchievement("RAPID_UNLOCK");
            }
        }
    }

    public void BounceCheckUnlock(bool homing, bool shooting, bool sitting, bool dashing)
    {
        // Defeat each enemy type in one shot
        if (!ReadWriteSaveManager.Instance.GetData("UnlockedBounce", false))
        {
            if (homing && shooting && sitting && dashing)
            {
                ReadWriteSaveManager.Instance.SetData("UnlockedBounce", true, true);
                if (steamBuild) SteamAchievements.UnlockAchievement("BOUNCE_UNLOCK");
            }
        }
    }

    public void HeaveCheck(int combo)
    {
        if (combo >= 5 && steamBuild) SteamAchievements.UnlockAchievement("TRI_HEAVE");
    }

    public void SpreadCheck(int hitTris)
    {
        if (hitTris >= 3 && steamBuild) SteamAchievements.UnlockAchievement("TRI_SPREAD");
    }

    public void HomingCheck(int combo)
    {
        if (combo >= 8 && steamBuild) SteamAchievements.UnlockAchievement("TRI_HOMING");
    }

    public void GiantCheck(float checkSizeX, float currentSizeX)
    {
        if (currentSizeX >= checkSizeX && steamBuild) SteamAchievements.UnlockAchievement("TRI_GIANT");
    }

    public void RicochetCheck(int combo)
    {
        if (combo >= 6 && steamBuild) SteamAchievements.UnlockAchievement("TRI_RICOCHET");
    }

    public void StackedCheck(int comboScore)
    {
        if (comboScore >= 3000 && steamBuild) SteamAchievements.UnlockAchievement("TRI_STACKED");
    }

    #endregion
}