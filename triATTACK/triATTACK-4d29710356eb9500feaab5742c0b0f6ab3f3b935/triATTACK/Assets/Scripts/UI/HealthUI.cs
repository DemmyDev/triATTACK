using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    Image[] tris;
    [SerializeField] Sprite fullTri;
    [SerializeField] Sprite emptyTri;

    void Start()
    {
        tris = GetComponentsInChildren<Image>();
        GameMaster.Instance.SetHealthUI(this);
    }

    public void DisableUI()
    {
        gameObject.SetActive(false);
    }

    public void SetHealthSprites(int health)
    {
        for (int i = 0; i < tris.Length; i++)
        {
            if (i < health)
            {
                tris[i].sprite = fullTri;
            }
            else
            {
                tris[i].sprite = emptyTri;
            }
        }
    }
}
