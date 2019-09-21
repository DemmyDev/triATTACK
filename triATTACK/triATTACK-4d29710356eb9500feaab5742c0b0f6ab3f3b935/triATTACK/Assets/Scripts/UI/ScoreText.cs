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
        GameMaster.Instance.SetScoreText(this);
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

    public void SetHighScore()
    {
        if (currentScore > ReadWriteSaveManager.Instance.GetData("HighScore", 0))
        {
            ReadWriteSaveManager.Instance.SetData("HighScore", 0, true);
            highScoreText.text = "tri.highScore = " + ReadWriteSaveManager.Instance.GetData("HighScore", 0).ToString() + ";";
        }
    }
}
