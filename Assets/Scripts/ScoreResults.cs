using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreResults : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI HighscoreText;

    float score = 0;
    float highscore = 0;

    public float Score //The score on the results screen
    {
        get => score;
        set
        {
            score = value;
            ScoreText.text = "Final Score : " + score.ToString();
        }
    }

    public float Highscore //The highscore on the results screen
    {
        get => highscore;
        set
        {
            highscore = value;
            HighscoreText.text = "Highscore : " + highscore.ToString();
        }
    }
}
