using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

    [SerializeField] int bulletNum;
    BulletSelect parentUI;

    void Start()
    {
        parentUI = transform.parent.GetComponent<BulletSelect>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriBullet"))
        {
            parentUI.SelectBullet(bulletNum);
        }
    }
}
