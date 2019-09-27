using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletCollision : MonoBehaviour
{
    ScoreText scoreText;
    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    [SerializeField] int addScoreSittingHit;
    [SerializeField] int addScoreDashingHit;
    int addScoreStacking = 0, comboCounter;
    bool defeatedHoming = false, defeatedShooting = false, defeatedSitting = false, defeatedDashing = false;
    int recallCounter;

    [SerializeField] Transform flashObj;
    [SerializeField] float startFreezeDuration;
    float freezeDuration;
    [SerializeField] float addFreezeDuration;
    float colorNum = .15f;

    public float pitch = .3f;

    bool isShaking = false;
    Bullet parentBul;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;
    ComboUI comboUI;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        parentBul = transform.parent.GetComponent<Bullet>();
        freezeDuration = startFreezeDuration;
        Invoke("ObjectShake", 4f);

        if (SceneManager.GetActiveScene().name == "tri.Attack")
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
            comboUI = FindObjectOfType<ComboUI>();
        }
    }

    void ObjectShake()
    {
        isShaking = true;
    }

    void Update()
    {
        if (isShaking && !parentBul.GetIsRecalling())
        {
            transform.localPosition = new Vector2(Random.Range(-.2f, .2f), Random.Range(-.2f, .2f));
        }
        else if (parentBul.GetIsRecalling())
        {
            transform.localPosition = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.Instance.ChangeBackColor(colorNum);
            if (colorNum < .35f)
            {
                colorNum += .05f;
            }

            GameMaster.Instance.Freeze(freezeDuration);
            if (freezeDuration < .15f)
            {
                freezeDuration += addFreezeDuration;
            }

            var flashInst = Instantiate(flashObj, other.transform.position, Quaternion.identity);
            Destroy(flashInst.gameObject, .25f);

            AudioManager.Instance.Play("EnemyHit", pitch);
            pitch += .05f;

            HomingEnemy homingEnemy = other.gameObject.GetComponent<HomingEnemy>();
            ShootingEnemy shootingEnemy = other.gameObject.GetComponent<ShootingEnemy>();
            SittingEnemy sittingEnemy = other.gameObject.GetComponent<SittingEnemy>();
            DashingEnemy dashingEnemy = other.gameObject.GetComponent<DashingEnemy>();

            Vector3 bulletPos = gameObject.transform.position;
            Quaternion bulletRot = gameObject.transform.rotation;

            if (homingEnemy)
            {
                scoreText.SetScore(addScoreHomingHit + addScoreStacking);
                comboUI.SetCounter(addScoreHomingHit + addScoreStacking, homingEnemy.transform.position);
                homingEnemy.DamageEnemy();
                defeatedHoming = true;
            }
            else if (shootingEnemy)
            {
                scoreText.SetScore(addScoreShootingHit + addScoreStacking);
                comboUI.SetCounter(addScoreShootingHit + addScoreStacking, shootingEnemy.transform.position);
                shootingEnemy.DamageEnemy();
                defeatedShooting = true;
            }
            else if (sittingEnemy)
            {
                scoreText.SetScore(addScoreSittingHit + addScoreStacking);
                comboUI.SetCounter(addScoreSittingHit + addScoreStacking, sittingEnemy.transform.position);
                sittingEnemy.DamageEnemy();
                defeatedSitting = true;
            }
            else if (dashingEnemy)
            {
                int dashingAdd = dashingEnemy.GetAddScore();
                scoreText.SetScore(addScoreDashingHit + addScoreStacking + dashingAdd);
                comboUI.SetCounter(addScoreDashingHit + addScoreStacking + dashingAdd, dashingEnemy.transform.position);
                dashingEnemy.DamageEnemy();
                defeatedDashing = true;
            }

            if (parentBul.GetIsRecalling()) recallCounter++;

            addScoreStacking += 100;
            comboCounter++;
            GameMaster.Instance.SpongeCheckUnlock(comboCounter);
            GameMaster.Instance.BounceCheckUnlock(defeatedHoming, defeatedShooting, defeatedSitting, defeatedDashing);
            GameMaster.Instance.HeaveCheck(recallCounter);
        }

        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();

            if (playerShooting.isRecalling || playerShooting.canRecall)
            {
                shake.Shake(shakeDuration, shakeIntensity);
                AudioManager.Instance.Play("PlayerTriHit");
                playerShooting.BulletHit();
                Destroy(parentBul.gameObject);
            }
        }
    }
}