using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBullet : MonoBehaviour
{
    [SerializeField] int bulletSpeed;

    bool isRecalling;
    Transform player, spriteObj;
    Animation anim;

    [SerializeField] float rotateSpeed;

    float screenX = 37.25f, screenY = 21.75f;

    Transform cursor;

    void Start ()
    {
        AudioManager.Instance.Play("PlayerShoot");
        cursor = FindObjectOfType<Crosshair>().transform;
        spriteObj = transform.Find("Sprite");
        anim = GameObject.Find("Anim").GetComponent<Animation>();
        isRecalling = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        transform.rotation = player.rotation;
        StartCoroutine(AutoRecall());
        anim.Play();
	}
	
	void Update ()
    {
        ScreenWrap();

        if (!isRecalling)
        {
            spriteObj.Rotate(transform.forward * Time.deltaTime * (rotateSpeed / 2));

            Vector2 cursorPos = cursor.position;
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

        if (pos.x > screenX) transform.position = new Vector2(-screenX, pos.y);
        if (pos.x < -screenX) transform.position = new Vector2(screenX, pos.y);
        if (pos.y > screenY) transform.position = new Vector2(pos.x, -screenY);
        if (pos.y < -screenY) transform.position = new Vector2(pos.x, screenY);
    }

    IEnumerator AutoRecall()
    {
        yield return new WaitForSeconds(3f);
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
