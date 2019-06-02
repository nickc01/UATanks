using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesDisplay : Display<int>
{
    TextMeshProUGUI Text; //The text object that represents the lives

    public override int Value //The value of the lives display
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
