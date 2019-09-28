using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicate : MonoBehaviour
{
	void Start ()
    {
        if (ReadWriteSaveManager.Instance.GetData("PlayedOnce", false, false)) gameObject.SetActive(false);
	}
}
