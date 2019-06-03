using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColorizer : MonoBehaviour
{
    TextMeshProUGUI text;
    private void OnEnable()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        text.color = GameManager.CurrentGameColor;
    }
}
