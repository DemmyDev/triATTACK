using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;

    bool isRecalling;
    Transform player, spriteObj;

    [SerializeField] float rotateSpeed;

    float screenX = 37.25f, screenY = 21.75f;
    TrailRenderer trail;

    void Start ()
    {
        spriteObj = transform.Find("Sprite");
        trail = transform.Find("Trail").GetComponent<TrailRenderer>();
        isRecalling = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.rotation = player.rotation;
        StartCoroutine(AutoRecall());
	}
	
	void Update ()
    {
        ScreenWrap();

        if (!isRecalling)
        {
            spriteObj.Rotate(transform.forward * Time.deltaTime * (rotateSpeed / 2));

            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector2.MoveTowards(transform.position, cursorPos, bulletSpeed * Time.deltaTime);
        }
        else
        {
            spriteObj.Rotate(transform.forward * Time.deltaTime * rotateSpeed);
            transform.position = Vector2.MoveTowards(transform.position, player.position, bulletSpeed * 3f * Time.deltaTime);
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
        yield return new WaitForSeconds(3f);
        if (!isRecalling)
        {
            AudioManager.Instance.Play("PlayerRecall");
            isRecalling = true;
        }
    }

    IEnumerator ResetTrail()
    {
        trail.time = 0;
        yield return new WaitForSeconds(.2f);
        trail.time = .5f;
    }

    public bool GetIsRecalling()
    {
        return isRecalling;
    }

    public void SetIsRecalling(bool recalling)
    {
        isRecalling = recalling;
    }
}
