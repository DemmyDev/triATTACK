using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpongeCollision : MonoBehaviour
{
    ScoreText scoreText;
    [SerializeField] int addScoreShootingHit;
    [SerializeField] int addScoreHomingHit;
    [SerializeField] int addScoreSittingHit;
    int addScoreStacking = 0, comboCounter;
    bool defeatedHoming = false, defeatedShooting = false, defeatedSitting = false;

    [SerializeField] Transform flashObj;
    [SerializeField] float startFreezeDuration;
    float freezeDuration;
    [SerializeField] float addFreezeDuration;
    float colorNum = .15f;

    public float pitch = .3f;

    bool isShaking = false;
    SpongeBullet parentBul;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] float scaleModifier;

    void Start()
    {
        shake = Camera.main.GetComponent<ScreenShake>();
        parentBul = transform.parent.GetComponent<SpongeBullet>();
        freezeDuration = startFreezeDuration;
        Invoke("ObjectShake", 4f);

        if (SceneManager.GetActiveScene().name == "tri.Attack")
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<ScoreText>();
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

    void IncreaseScale()
    {
        transform.localScale = new Vector2(transform.localScale.x + scaleModifier, transform.localScale.y + scaleModifier);
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

            IncreaseScale();

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

            Vector3 bulletPos = gameObject.transform.position;
            Quaternion bulletRot = gameObject.transform.rotation;

            if (homingEnemy)
            {
                scoreText.SetScore(addScoreHomingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreHomingHit + addScoreStacking, homingEnemy.transform.position);
                homingEnemy.DamageEnemy();
                defeatedHoming = true;
            }
            else if (shootingEnemy)
            {
                scoreText.SetScore(addScoreShootingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreShootingHit + addScoreStacking, shootingEnemy.transform.position);
                shootingEnemy.DamageEnemy(bulletPos, bulletRot);
                defeatedShooting = true;
            }
            else if (sittingEnemy)
            {
                scoreText.SetScore(addScoreSittingHit + addScoreStacking);
                FindObjectOfType<ComboUI>().SetCounter(addScoreSittingHit + addScoreStacking, sittingEnemy.transform.position);
                sittingEnemy.DamageEnemy(bulletPos, bulletRot);
                defeatedSitting = true;
            }

            addScoreStacking += 100;

            comboCounter++;
            GameMaster.Instance.SpongeCheckUnlock(comboCounter);
            GameMaster.Instance.BounceCheckUnlock(defeatedHoming, defeatedShooting, defeatedSitting);
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
