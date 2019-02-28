using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    int currentScore;
    Text scoreText;

    void Start ()
    {
        currentScore = 0;
        scoreText = GetComponent<Text>();
        scoreText.text = "tri.score = " + currentScore.ToString() + ";"; 
	}

    public void SetScore(int addScore)
    {
        currentScore = currentScore + addScore;
        scoreText.text = "tri.score = " + currentScore.ToString() + ";";
    }

    public void MoveText()
    {
        RectTransform rectTransform = scoreText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(.5f, .5f);
        rectTransform.anchorMax = new Vector2(.5f, .5f);
        rectTransform.anchoredPosition = new Vector3(0f, 30f, 0f);
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = new Color(255f, 255f, 255f);
        scoreText.CrossFadeAlpha(2f, 0f, true);
        scoreText.fontSize = 35;
    }
}
