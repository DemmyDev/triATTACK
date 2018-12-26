using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    private int currentScore;
    private Text scoreText;

    void Start ()
    {
        currentScore = 0;
        scoreText = GetComponent<Text>();
        scoreText.text = "triSCORE = " + currentScore.ToString(); 
	}

    public void SetScore(int addScore)
    {
        currentScore = currentScore + addScore;
        scoreText.text = "triSCORE = " + currentScore.ToString();
    }

    public void MoveText()
    {
        RectTransform rectTransform = scoreText.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0f, -10f, 0f);
        scoreText.alignment = TextAnchor.MiddleCenter;
        scoreText.color = new Color(255f, 255f, 255f);
    }
}
