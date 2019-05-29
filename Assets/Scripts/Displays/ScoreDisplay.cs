using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour, IIsPlayerSpecific
{
    private static ScoreDisplay Singleton; //The main singleton of the score display
    private TextMeshProUGUI Text; //The text object that represents the score
    private float scoreInternal = 0.0f; //The internal variable for storing the score
    private string baseText; //The base text that is inserted before the score number

    public static float Score
    {
        get => Singleton.scoreInternal;
        set
        {
            Singleton.scoreInternal = value;
            //Update the score display
            Singleton.Text.text = Singleton.baseText + value.ToString();
        }
    }

    public int PlayerID { get; set; }

    private void Start()
    {
        //Set the singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Get the text object
        Text = GetComponent<TextMeshProUGUI>();
        baseText = Text.text;
        Text.text = baseText + scoreInternal.ToString();
    }
}
