using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnabler : MonoBehaviour
{
    bool Started = false;

    UIManager Manager;
    private void Start()
    {
        if (Started)
        {
            return;
        }
        Started = true;
        Manager = GetComponentInParent<UIManager>();
        Debug.Log("Manager = " + Manager);
    }

    private void OnEnable()
    {
        if (!Started)
        {
            Start();
        }
        foreach (var button in GetComponentsInChildren<Button>(true))
        {
            button.gameObject.SetActive(Manager.ButtonsEnabled);
        }
    }
}
