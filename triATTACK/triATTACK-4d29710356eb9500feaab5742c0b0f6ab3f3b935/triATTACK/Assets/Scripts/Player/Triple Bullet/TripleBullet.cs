using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TripleBullet : MonoBehaviour {

    [HideInInspector] public bool isRecalling;
    [SerializeField] List<TripleCollision> bullets;
    int recalledBullets = 0;

    int hitTris;

    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    [SerializeField] int addScoreSittingHit;
    [SerializeField] int addScoreDashingHit;
    int addScoreStacking = 0, comboCounter;
    bool defeatedHoming = false, defeatedShooting = false, defeatedSitting = false, defeatedDashing = false;

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

    void Start ()
    {
        AudioManager.Instance.Play("PlayerShoot");
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
    }

    void Update ()
    {
        // Movement
	    if (!isRecalling)
        {
            foreach (TripleCollision bullet in bullets) bullet.NormalMove();
        }
        else
        {
            foreach (TripleCollision bullet in bullets) bullet.RecallMove();
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

    void ObjectShake()
    {
        isShaking = true;
    }

    public void BulletRecalled(PlayerShooting player)
    {
        recalledBullets++;
        shake.Shake(shakeDuration, shakeIntensity);

        if (recalledBullets == 3)
        {
            player.BulletHit();
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
            foreach (TripleCollision bullet in bullets) bullet.PlayAnim();
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

    public void TrackTripleHits()
    {
        hitTris++;
        GameMaster.Instance.SpreadCheck(hitTris);
    }

    public void EnemyHit(GameObject enemy, Vector3 bulletPos, Quaternion bulletRot)
    {
        GameMaster.Instance.Freeze(freezeDuration);
        if (freezeDuration < .15f) freezeDuration += addFreezeDuration;

        if (ReadWriteSaveManager.Instance.GetData("CanFlash", true, false))
        {
            var flashInst = Instantiate(flashObj, bulletPos, Quaternion.identity);
            Destroy(flashInst.gameObject, .25f);

            GameMaster.Instance.ChangeBackColor(colorNum);
            if (colorNum < .35f)
            {
                colorNum += .05f;
            }
        }

        AudioManager.Instance.Play("EnemyHit", hitPitch);
        hitPitch += .05f;

        HomingEnemy homing = enemy.GetComponent<HomingEnemy>();
        ShootingEnemy shooting = enemy.GetComponent<ShootingEnemy>();
        SittingEnemy sitting = enemy.GetComponent<SittingEnemy>();
        DashingEnemy dashing = enemy.GetComponent<DashingEnemy>();

        if (homing)
        {
            scoreText.SetScore(addScoreHomingHit + addScoreStacking);
            comboUI.SetCounter(addScoreHomingHit + addScoreStacking, homing.transform.position);
            homing.DamageEnemy();
            defeatedHoming = true;
        }
        else if (shooting)
        {
            scoreText.SetScore(addScoreShootingHit + addScoreStacking);
            comboUI.SetCounter(addScoreShootingHit + addScoreStacking, shooting.transform.position);
            shooting.DamageEnemy();
            defeatedShooting = true;
        }
        else if (sitting)
        {
            scoreText.SetScore(addScoreSittingHit + addScoreStacking);
            comboUI.SetCounter(addScoreSittingHit + addScoreStacking, sitting.transform.position);
            sitting.DamageEnemy();
            defeatedSitting = true;
        }
        else if (dashing)
        {
            int dashingAdd = dashing.GetAddScore();
            scoreText.SetScore(addScoreDashingHit + addScoreStacking + dashingAdd);
            comboUI.SetCounter(addScoreDashingHit + addScoreStacking + dashingAdd, dashing.transform.position);
            dashing.DamageEnemy();
            defeatedDashing = true;
        }

        addScoreStacking += 100;
        comboCounter++;
        GameMaster.Instance.SpongeCheckUnlock(comboCounter);
        GameMaster.Instance.BounceCheckUnlock(defeatedHoming, defeatedShooting, defeatedSitting, defeatedDashing);
    }
}
