using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathText : MonoBehaviour
{
    void Start()
    {
        GameMaster.Instance.SetDeathText(this);
        gameObject.SetActive(false);
    }
}