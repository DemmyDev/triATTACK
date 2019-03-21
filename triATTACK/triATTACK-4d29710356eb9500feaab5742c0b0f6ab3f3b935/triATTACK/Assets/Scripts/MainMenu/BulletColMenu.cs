using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletColMenu : MonoBehaviour
{
    [SerializeField] Transform flashObj;
    [SerializeField] float startFreezeDuration;
    float freezeDuration;
    [SerializeField] float addFreezeDuration;
    float colorNum = .15f;

    public float pitch = .3f;

    void Start()
    {
        freezeDuration = startFreezeDuration;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameMaster.gm.ChangeBackColor(colorNum);

            GameMaster.gm.Freeze(freezeDuration);
            if (freezeDuration < .15f)
            {
                freezeDuration += addFreezeDuration;
            }

            // Make the flash object animate
            var flashInst = Instantiate(flashObj, other.transform.position, Quaternion.identity);
            Destroy(flashInst.gameObject, .25f);

            FindObjectOfType<AudioManager>().Play("TextEnter");
        }
    }
}