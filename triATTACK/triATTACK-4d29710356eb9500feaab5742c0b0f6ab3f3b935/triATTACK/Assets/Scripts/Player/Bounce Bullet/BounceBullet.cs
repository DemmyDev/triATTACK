using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;
    float shootSpeed;

    [HideInInspector] public bool isRecalling;
    Transform target;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] float rotateSpeed;
    [SerializeField] float slowDownDivider = 1.01f;
    [HideInInspector] public float slowDownSpeed;
    Transform spriteObj;
    Animation anim;

    float screenX = 37.25f, screenY = 21.75f;

    Vector2 direction;

    void Start()
    {
        direction = new Vector2(transform.up.x, transform.up.y);
        shootSpeed = bulletSpeed;
        AudioManager.Instance.Play("PlayerShoot");
        anim = GameObject.Find("Anim").GetComponent<Animation>();
        spriteObj = GameObject.Find("Sprite").GetComponent<Transform>();
        isRecalling = false;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        transform.rotation = target.rotation;
        slowDownSpeed = rotateSpeed;

        StartCoroutine(AutoRecall());
        anim.Play();
    }

    void Update()
    {
        ScreenWrap();
    }

    void FixedUpdate()
    {
        if (!isRecalling)
        {
            rb.velocity = direction * shootSpeed;
            shootSpeed /= 1.02f;
            spriteObj.Rotate(transform.forward * Time.deltaTime * slowDownSpeed);
            slowDownSpeed /= slowDownDivider;
        }
        else
        {
            spriteObj.Rotate(Vector3.forward * Time.deltaTime * rotateSpeed);
            transform.position = Vector2.MoveTowards(transform.position, target.position, bulletSpeed * 2f * Time.deltaTime);
        }

        if (target.transform.position == transform.position && isRecalling) spriteObj.GetComponent<BounceCollision>().PlayerCollide(target.GetComponent<PlayerShooting>());
    }

    void ScreenWrap()
    {
        Vector2 pos = transform.position;

        if (pos.x > screenX) transform.position = new Vector2(-screenX, pos.y);
        if (pos.x < -screenX) transform.position = new Vector2(screenX, pos.y);
        if (pos.y > screenY) transform.position = new Vector2(pos.x, -screenY);
        if (pos.y < -screenY) transform.position = new Vector2(pos.x, screenY);
    }

    public void Bounce(Vector2 enemyNormal)
    {
        direction = Vector2.Reflect(direction, enemyNormal);
    }

    IEnumerator AutoRecall()
    {
        yield return new WaitForSeconds(5f);
        if (!isRecalling)
        {
            AudioManager.Instance.Play("PlayerRecall");
            SetIsRecalling(true);
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
            anim.Stop();
            anim.Play();
        }
    }
}