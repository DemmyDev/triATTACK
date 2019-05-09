using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripleCollision : MonoBehaviour {

    // Screen Wrapping
    // Movement and rotation
    // Recall trigger with player
    // Add recalledBullets to TripleBullet parent
    // Auto Recall
    // Trail stuff

    [SerializeField] int bulletSpeed;
    float slowDownDivider = 1.01f;
    [HideInInspector] public float slowDownSpeed;
    [HideInInspector] public Rigidbody2D rb;

    float screenX = 37.25f, screenY = 21.75f;
    TrailRenderer trail;

    Transform target;

    TripleBullet parentBul;

    bool isShaking = false;

	void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        parentBul = transform.parent.GetComponent<TripleBullet>();

        // How do we do direction??


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
        // Movement at -45, 0, or 45 degree angle (compared to parent)
        // Slowing movement


	}

    // Called every frame in TripleBullet, if it is recalling
    public void RecallMove()
    {
        // Rotation
        // Movement directly towards player object (world space)
    }

    void ObjectShake()
    {
        isShaking = true;
    }

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.2f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            parentBul.EnemyHit(other.gameObject, transform.position, transform.rotation);
        }
    }
}
