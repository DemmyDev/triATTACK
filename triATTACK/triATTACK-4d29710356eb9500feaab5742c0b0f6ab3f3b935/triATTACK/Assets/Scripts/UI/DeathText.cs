using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeathText : MonoBehaviour
{
    [SerializeField] GameObject menuButton;
    [SerializeField] EventSystem eventSystem;

    void Start()
    {
        GameMaster.Instance.SetDeathText(this);
    }

    public void EnableText()
    {
        CanvasGroup canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 1f;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
        eventSystem.SetSelectedGameObject(menuButton);
    }
}