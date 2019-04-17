using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;

    public static bool isRecalling;
    Transform target;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] float rotateSpeed;
    [SerializeField] float slowDownDivider = 1.01f;
    [HideInInspector] public float slowDownSpeed;
    Transform spriteObj;

    bool dumbFix = true;

    float screenX = 37.25f, screenY = 21.75f;
    TrailRenderer trail;

    void Start()
    {
        spriteObj = GameObject.Find("Sprite").GetComponent<Transform>(); ;
        isRecalling = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.rotation = target.rotation;

        Vector2 direction = new Vector2(transform.up.x, transform.up.y);
        rb.velocity = direction * bulletSpeed;
        slowDownSpeed = rotateSpeed;

        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        StartCoroutine(AutoRecall());
    }

    void Update()
    {
        ScreenWrap();

        if (!isRecalling)
        {
            if (slowDownSpeed > 10f)
            {
                spriteObj.Rotate(Vector3.forward * Time.deltaTime * slowDownSpeed);
                slowDownSpeed /= slowDownDivider;
            }
            else
            {
                isRecalling = true;
            }
        }
        else if (isRecalling)
        {
            // Just to prevent velocity being set to zero each frame
            if (dumbFix)
            {
                rb.velocity = Vector2.zero;
                dumbFix = false;
                // im a bad programmer ayy
            }
            
            spriteObj.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
            transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * 2f * Time.deltaTime );

            Vector3 difference = target.transform.position - transform.position;
            difference.Normalize();
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);
        }
    }

    void ScreenWrap()
    {
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

        if (pos.y < -screenY)
        {
            trail.Clear();
            StartCoroutine(ResetTrail());
            transform.position = new Vector2(pos.x, screenY);
        }
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

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.2f);
        trail.time = .5f;
    }
}