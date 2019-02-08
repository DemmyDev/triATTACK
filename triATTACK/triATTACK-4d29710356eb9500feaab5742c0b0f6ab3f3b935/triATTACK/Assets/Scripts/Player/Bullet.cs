using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;

    public static bool isRecalling;
    Transform target;
    Rigidbody2D rb;

    [SerializeField] float rotateSpeed;
    [SerializeField] float slowDownDivider = 1.01f;
    float slowDownSpeed;
    Transform spriteObj;

    void Start()
    {
        spriteObj = GameObject.Find("Sprite").GetComponent<Transform>(); ;
        isRecalling = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.rotation = target.rotation;

        Vector2 direction = new Vector2(transform.up.x, transform.up.y);
        rb.velocity = direction * Time.deltaTime * bulletSpeed;
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
            spriteObj.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
            rb.velocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, target.position, (bulletSpeed / 25) * Time.deltaTime );

            Vector3 difference = target.transform.position - transform.position;
            difference.Normalize();
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90f);
        }
    }
}