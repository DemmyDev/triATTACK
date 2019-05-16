using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleCollision : MonoBehaviour {

    // Movement and rotation
    // Recall trigger with player
    // Add recalledBullets to TripleBullet parent
    // Auto Recall
    // Trail stuff

    [SerializeField] int bulletSpeed = 28;

    [SerializeField] float rotateSpeed = 700;
    float slowDownDivider = 1.01f;
    [HideInInspector] public float slowDownSpeed;
    [HideInInspector] public Rigidbody2D rb;

    float screenX = 37.25f, screenY = 21.75f;
    TrailRenderer trail;

    GameObject player;
    Transform target;

    TripleBullet parentBul;

    bool isShaking = false;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        parentBul = transform.parent.GetComponent<TripleBullet>();
        slowDownSpeed = rotateSpeed;

        Vector2 direction = new Vector2(transform.right.x, transform.right.y);
        rb.velocity = direction * bulletSpeed;
	}
    

    void Update()
    {
        // Screen wrapping
        Vector2 pos = transform.position;

        if (pos.x > screenX)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(-screenX, pos.y);
        }

        if (pos.x < -screenX)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(screenX, pos.y);
        }

        if (pos.y > screenY)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, -screenY);
        }

        if (pos.y < - screenY)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, screenY);
        }
    }

    // Called every frame in TripleBullet, if it is not recalling
    public void NormalMove ()
    {
        // Rotation
        // Slowing rotation
        // Movement
        // Slowing movement
        if (slowDownSpeed > 10f)
        {
            transform.Rotate(Vector3.forward * Time.deltaTime * slowDownSpeed);
            slowDownSpeed /= slowDownDivider;
        }
        else parentBul.SetIsRecalling(true);
	}

    // Called every frame in TripleBullet, if it is recalling
    public void RecallMove()
    {
        // Rotation
        // Movement directly towards player object (world space)
        transform.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
        transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * 2f * Time.deltaTime);
    }

    void ObjectShake()
    {
        isShaking = true;
    }

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.2f);
        trail.time = .5f;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            parentBul.EnemyHit(other.gameObject, transform.position, transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();

            if (playerShooting.isRecalling || playerShooting.canRecall)
            {
                AudioManager.am.Play("PlayerTriHit");
                parentBul.BulletRecalled(playerShooting);
                gameObject.SetActive(false);
            }
        }
    }
}
