using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
}

