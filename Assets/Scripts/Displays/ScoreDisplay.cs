using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Display<ValueType> : MonoBehaviour,IIsPlayerSpecific
{
    private ValueType value = default; //The internal variable for storing the score

    public int PlayerID { get; set; }

    public virtual ValueType Value
    {
        get => value;
        set
        {
            this.value = value;
        }
    }

    protected virtual void Start() { }
}

public class ScoreDisplay : Display<float>
{
    TextMeshProUGUI Text; //The text object that represents the score

    public override float Value
    {
        get => base.Value;
        set
        {
            if (Text == null)
            {
                Text = GetComponent<TextMeshProUGUI>();
            }
            Text.text = (base.Value = value).ToString();
        }
        //set => Text.text = (base.Value = value).ToString();
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    /*private TextMeshProUGUI Text; //The text object that represents the score
    private float scoreInternal = 0.0f; //The internal variable for storing the score
    private string baseText; //The base text that is inserted before the score number

    public float Score
    {
        get => scoreInternal;
        set
        {
            scoreInternal = value;
            //Update the score display
            Text.text = baseText + value.ToString();
        }
    }

    public int PlayerID { get; set; }

    private void Start()
    {
        //Get the text object
        Text = GetComponent<TextMeshProUGUI>();
        baseText = Text.text;
        Text.text = baseText + scoreInternal.ToString();
    }*/
}
