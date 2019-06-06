using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ColorShade
{
    Light,
    Normal,
    Dark
}

public class TextColorizer : MonoBehaviour
{
    [SerializeField] ColorShade Shade = ColorShade.Normal;
    TextMeshProUGUI text;
    private void OnEnable()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        switch (Shade)
        {
            case ColorShade.Light:
                text.color = GameManager.CurrentGameColorBright;
                break;
            case ColorShade.Normal:
                text.color = GameManager.CurrentGameColor;
                break;
            case ColorShade.Dark:
                text.color = GameManager.CurrentGameColorDark;
                break;
            default:
                break;
        }
    }
}
