using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Highscore
{
    public static float GetScoreFor(int PlayerNumber)
    {
        return PlayerPrefs.GetFloat("HighscorePlayer" + PlayerNumber);
    }

    public static void SetScoreFor(int PlayerNumber,float score)
    {
        PlayerPrefs.SetFloat("HighscorePlayer" + PlayerNumber,score);
    }
}
