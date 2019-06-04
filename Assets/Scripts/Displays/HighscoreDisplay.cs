using System;
using TMPro;
using UnityEngine;

public class HighscoreDisplay : Display<float>
{
    TextMeshProUGUI Text; //The text object that represents the highscore
    float originalValue;

    public override float Value //The value of the highscore display
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

    public override void SetDirect(float value)
    {
        base.SetDirect(value);
        originalValue = Value;
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }

    protected override void InterpolateTo(float newValue, float Speed)
    {
        originalValue = Mathf.Lerp(originalValue, newValue, Speed * Time.deltaTime);
        Text.text = (Mathf.Round(originalValue * 100f) / 100f).ToString();
    }
}

