using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {

    int currentScore;
    Text scoreText;

    [SerializeField] Text highScoreText;

    void Start ()
    {
        currentScore = 0;
        scoreText = GetComponent<Text>();
        scoreText.text = "tri.score = " + currentScore.ToString() + ";";
        highScoreText.text = "tri.highScore = " + PlayerPrefs.GetInt("HighScore", 0).ToString() + ";";
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

    public void SetHighScore()
    {
        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            highScoreText.text = "tri.highScore = " + PlayerPrefs.GetInt("HighScore", 0).ToString() + ";";
        }
    }
}
