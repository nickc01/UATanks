using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    Button button;
    UIManager SourceUI;

    private void Start()
    {
        button = GetComponent<Button>();
        SourceUI = GetComponentInParent<UIManager>();
        button.onClick.AddListener(() => 
        {
            Callbacks.Resume(SourceUI);
        });
    }
}
