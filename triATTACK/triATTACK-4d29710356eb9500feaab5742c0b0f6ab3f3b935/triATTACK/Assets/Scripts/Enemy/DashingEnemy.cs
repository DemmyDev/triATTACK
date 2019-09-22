using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashingEnemy : MonoBehaviour
{
    [SerializeField] int startSpeed;
    [SerializeField] int addSpeed;
    int currentSpeed = 0;
    int addScore = 0;

    Transform target;

    [Range(0f, 2f)]
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeDuration;
    ScreenShake shake;

    [SerializeField] Transform dashingParticlePrefab;

    bool isDashing = false;
    Animator anim;
    Vector3 normDirection;

    float screenX = 36.25f, screenY = 20.75f;

    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        shake = Camera.main.GetComponent<ScreenShake>();
        StartCoroutine(BeginMove(true));
    }

    void Update()
    {
        ScreenWrap();

        if (isDashing)
        {
            transform.position += (normDirection * currentSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 difference = target.transform.position - transform.position;
            difference.Normalize();
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
        }
    }

    void ScreenWrap()
    {
        Vector2 pos = transform.position;
        if (pos.x > screenX) transform.position = new Vector2(-screenX, pos.y);
        else if (pos.x < -screenX) transform.position = new Vector2(screenX, pos.y);
        else if (pos.y > screenY) transform.position = new Vector2(pos.x, -screenY);
        else if (pos.y < -screenY) transform.position = new Vector2(pos.x, screenY);
    }

    void AddSpeed()
    {
        StartCoroutine(BeginMove(false));
    }

    IEnumerator BeginMove(bool hasJustSpawned)
    {
        anim.SetTrigger("IndicateDash");
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("Dash");
        isDashing = true;
        if (hasJustSpawned)
        {
            currentSpeed = startSpeed;
            normDirection = (target.position - transform.position).normalized;
        }
        else
        {
            addScore += 100;
            currentSpeed += addSpeed;
        }
        if (currentSpeed < 42) Invoke("AddSpeed", 4.5f);
    }

    public int GetAddScore()
    {
        return addScore;
    }

    public void DamageEnemy()
    {
        Instantiate(dashingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
        shake.Shake(shakeDuration, shakeIntensity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Instantiate(dashingParticlePrefab, gameObject.transform.position, gameObject.transform.rotation);
            Player player = other.gameObject.GetComponent<Player>();
            player.DamagePlayer();
            Destroy(gameObject);
        }
    }
}