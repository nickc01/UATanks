using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesDisplay : MonoBehaviour
{
    private static LivesDisplay Singleton; //The main singleton of the lives display
    private TextMeshProUGUI Text; //The text object that represents the lives
    private float livesInternal = 0.0f; //The internal variable for storing the lives
    private string baseText; //The base text that is inserted before the lives number

    public static float Lives
    {
        get => Singleton.livesInternal;
        set
        {
            Singleton.livesInternal = value;
            //Update the lives display
            Singleton.Text.text = Singleton.baseText + value.ToString();
        }
    }

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
        Text.text = baseText + livesInternal.ToString();
    }
}
