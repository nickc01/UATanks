using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreResults : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI HighscoreText;

    string ScoreBase;
    string HighscoreBase;
    float score = 0;
    float highscore = 0;

    public float Score
    {
        get => score;
        set
        {
            if (ScoreBase == null)
            {
                ScoreBase = ScoreText.text;
            }
            score = value;
            ScoreText.text = ScoreBase + score.ToString();
        }
    }

    public float Highscore
    {
        get => highscore;
        set
        {
            if (HighscoreBase == null)
            {
                HighscoreBase = HighscoreText.text;
            }
            highscore = value;
            HighscoreText.text = HighscoreBase + highscore.ToString();
        }
    }
}
