using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnabler : MonoBehaviour
{
    bool Started = false;

    UIManager UI; //The UI that this button is controlled by
    private void Start()
    {
        if (Started)
        {
            return;
        }
        Started = true;
        UI = GetComponentInParent<UIManager>();
    }

    private void OnEnable()
    {
        if (!Started)
        {
            Start();
        }
        //Enable or disable the buttons based on the value in the UI
        foreach (var button in GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(UI.ButtonsEnabled);
        }
    }
}
