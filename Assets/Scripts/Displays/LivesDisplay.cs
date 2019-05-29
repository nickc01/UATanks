using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesDisplay : Display<float>
{
    TextMeshProUGUI Text; //The text object that represents the score

    public override float Value
    {
        get => base.Value;
        //set => Text.text = (base.Value = value).ToString();
        set
        {
            if (Text == null)
            {
                Text = GetComponent<TextMeshProUGUI>();
            }
            Text.text = (base.Value = value).ToString();
        }
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
    /*private TextMeshProUGUI Text; //The text object that represents the lives
    private float livesInternal = 0.0f; //The internal variable for storing the lives
    private string baseText; //The base text that is inserted before the lives number

    public int PlayerID { get; set; }

    public float Lives
    {
        get => livesInternal;
        set
        {
            livesInternal = value;
            //Update the lives display
            Text.text = baseText + value.ToString();
        }
    }

    private void Start()
    {
        //Get the text object
        Text = GetComponent<TextMeshProUGUI>();
        baseText = Text.text;
        Text.text = baseText + livesInternal.ToString();
    }*/
}
