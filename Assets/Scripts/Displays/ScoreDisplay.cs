using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : Display<float>
{
    TextMeshProUGUI Text; //The text object that represents the score
    float originalValue;

    public override float Value
    {
        get => base.Value;
        set
        {
            if (Text == null)
            {
                Text = GetComponent<TextMeshProUGUI>();
            }
            base.Value = value;
            if (!Interpolate)
            {
                Text.text = value.ToString();
                originalValue = value;
            }
        }
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    public override void SetDirect(float value)
    {
        base.SetDirect(value);
        originalValue = Value;
    }

    protected override void InterpolateTo(float newValue, float Speed)
    {
        originalValue = Mathf.Lerp(originalValue, newValue, Speed * Time.deltaTime);
        Text.text = (Mathf.Round(originalValue * 100f) / 100f).ToString();
    }
}

