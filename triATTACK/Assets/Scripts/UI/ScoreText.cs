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
}
