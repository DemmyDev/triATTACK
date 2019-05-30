using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleBullet : MonoBehaviour {

    [HideInInspector] public bool isRecalling;
    [SerializeField] List<TripleCollision> bullets;
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

    void Start ()
    {
        originalPos = transform.position;
        shake = Camera.main.GetComponent<ScreenShake>();
        scoreText = FindObjectOfType<ScoreText>().GetComponent<ScoreText>();
        comboUI = FindObjectOfType<ComboUI>().GetComponent<ComboUI>();

        freezeDuration = startFreezeDuration;
        isRecalling = false;
        StartCoroutine(AutoRecall());
        Invoke("ObjectShake", 4f);
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
    }

    IEnumerator AutoRecall()
    {
        yield return new WaitForSeconds(5f);
        if (!isRecalling)
        {
            FindObjectOfType<AudioManager>().Play("PlayerRecall");
            isRecalling = true; 
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
