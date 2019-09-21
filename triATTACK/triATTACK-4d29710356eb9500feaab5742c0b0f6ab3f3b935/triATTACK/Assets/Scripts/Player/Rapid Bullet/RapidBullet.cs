using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RapidBullet : MonoBehaviour
{
    [HideInInspector] public bool isRecalling;
    [SerializeField] List<RapidCollision> bullets;
    int recalledBullets = 0;

    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    [SerializeField] int addScoreSittingHit;
    int addScoreStacking = 0;

    [SerializeField] float startFreezeDuration;
    [SerializeField] float addFreezeDuration;
    float freezeDuration;

    [SerializeField] Transform flashObj;

    float colorNum = .15f;

    [SerializeField] float hitPitch = .3f;

    ScoreText scoreText;
    ComboUI comboUI;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    bool isShaking = false;
    Vector2 originalPos;

    Transform player;
    float timeBtwSpawns = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        originalPos = transform.position;
        shake = Camera.main.GetComponent<ScreenShake>();
        freezeDuration = startFreezeDuration;
        isRecalling = false;
        StartCoroutine(AutoRecall());
        Invoke("ObjectShake", 4f);

        if (SceneManager.GetActiveScene().name == "tri.Attack")
        {
            scoreText = FindObjectOfType<ScoreText>().GetComponent<ScoreText>();
            comboUI = FindObjectOfType<ComboUI>().GetComponent<ComboUI>();
        }
        SpawnChildBullets();
    }

    void FixedUpdate()
    {
        // Movement
        if (!isRecalling)
        {
            foreach (RapidCollision bullet in bullets)
            {
                if (bullet.enabled == true) bullet.NormalMove();
            }
        }
        else
        {
            foreach (RapidCollision bullet in bullets)
            {
                if (bullet.enabled == true) bullet.RecallMove();
            }
        }

        // Shaking
        if (isShaking && !isRecalling)
        {
            Vector2 shakePos = new Vector2(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));
            shakePos += originalPos;
            transform.position = shakePos;
        }
        else if (isRecalling)
        {
            transform.position = originalPos;
        }
    }

    void SpawnChildBullets()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            StartCoroutine(ActivateChild(timeBtwSpawns, i));
            timeBtwSpawns += .1f;
        }
    }

    IEnumerator ActivateChild(float waitTime, int bulletCount)
    {
        yield return new WaitForSeconds(waitTime);

        AudioManager.Instance.Play("PlayerShoot");
        RapidCollision spawnBul = bullets[bulletCount];
        spawnBul.gameObject.SetActive(true);
        spawnBul.transform.position = player.GetComponent<PlayerShooting>().GetFirepointPos();
        Vector3 playerRot = player.GetComponent<PlayerShooting>().GetPlayerRot();
        spawnBul.transform.eulerAngles = new Vector3(playerRot.x, playerRot.y, playerRot.z + 90f);
        spawnBul.PlayAnim();
        shake.Shake(shakeDuration, shakeIntensity / 3);
    }

    void ObjectShake()
    {
        isShaking = true;
    }

    public void BulletRecalled(PlayerShooting playerShooting)
    {
        recalledBullets++;
        shake.Shake(shakeDuration, shakeIntensity);

        if (recalledBullets == 4)
        {
            playerShooting.BulletHit();
            Destroy(gameObject);
        }
    }

    public bool GetIsRecalling()
    {
        return isRecalling;
    }

    public void SetIsRecalling(bool recalling)
    {
        isRecalling = recalling;
        if (isRecalling)
        {
            foreach (RapidCollision bullet in bullets) bullet.PlayAnim();
        }
    }

    IEnumerator AutoRecall()
    {
        yield return new WaitForSeconds(5f);
        if (!isRecalling)
        {
            FindObjectOfType<AudioManager>().Play("PlayerRecall");
            SetIsRecalling(true);
        }
    }

    public void EnemyHit(GameObject enemy, Vector3 bulletPos, Quaternion bulletRot)
    {
        GameMaster.Instance.ChangeBackColor(colorNum);
        if (colorNum < .35f) colorNum += .05f;

        GameMaster.Instance.Freeze(freezeDuration);
        if (freezeDuration < .15f) freezeDuration += addFreezeDuration;

        var flashInst = Instantiate(flashObj, enemy.transform.position, Quaternion.identity);
        Destroy(flashInst.gameObject, .25f);

        AudioManager.Instance.Play("EnemyHit", hitPitch);
        hitPitch += .05f;

        HomingEnemy homing = enemy.GetComponent<HomingEnemy>();
        ShootingEnemy shooting = enemy.GetComponent<ShootingEnemy>();
        SittingEnemy sitting = enemy.GetComponent<SittingEnemy>();

        if (homing)
        {
            scoreText.SetScore(addScoreHomingHit + addScoreStacking);
            comboUI.SetCounter(addScoreHomingHit + addScoreStacking, homing.transform.position);
            homing.DamageEnemy();
        }
        else if (shooting)
        {
            scoreText.SetScore(addScoreShootingHit + addScoreStacking);
            comboUI.SetCounter(addScoreShootingHit + addScoreStacking, shooting.transform.position);
            shooting.DamageEnemy(bulletPos, bulletRot);
        }
        else if (sitting)
        {
            scoreText.SetScore(addScoreSittingHit + addScoreStacking);
            comboUI.SetCounter(addScoreSittingHit + addScoreStacking, sitting.transform.position);
            sitting.DamageEnemy(bulletPos, bulletRot);
        }

        addScoreStacking += 100;
    }
}