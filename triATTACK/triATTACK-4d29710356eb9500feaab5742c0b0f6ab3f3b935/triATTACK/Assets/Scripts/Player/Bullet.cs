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
    }

    void Update()
    {
        if (!isRecalling)
        {
            if (slowDownSpeed > 10f)
            {
                slowDownSpeed /= slowDownDivider;
                spriteObj.Rotate(Vector3.forward * Time.deltaTime * slowDownSpeed);
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
}