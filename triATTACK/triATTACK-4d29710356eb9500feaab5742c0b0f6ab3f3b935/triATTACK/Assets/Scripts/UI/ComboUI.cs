using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    [SerializeField] float posChangeValue;
    int comboCounter = 0;
    Text comboText;
    Animation anim;

	void Start()
    {
        comboText = GetComponent<Text>();
        anim = GetComponent<Animation>();
	}

    public void SetCounter(int add, Vector2 enemyPos)
    {
        Vector2 setPos = new Vector2(0f, 0f);

        if (enemyPos.x < 0)
        {
            setPos.x = enemyPos.x + posChangeValue;
        } 
        else
        {
            setPos.x = enemyPos.x - posChangeValue;
        }

        if (enemyPos.y < 0)
        {
            setPos.y = enemyPos.y + posChangeValue;
        }
        else
        {
            setPos.y = enemyPos.y - posChangeValue;
        }

        anim.Stop();
        transform.position = setPos;
        comboCounter += add;
        comboText.text = "+" + comboCounter.ToString();
        anim.Play();
    }

    public void ResetCounter()
    {
        comboCounter = 0;
    }
}