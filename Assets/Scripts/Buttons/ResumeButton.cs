using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    Button button; //The resume button
    UIManager SourceUI; //The source UI

    private void Start()
    {
        //Get the components
        button = GetComponent<Button>();
        SourceUI = GetComponentInParent<UIManager>();
        //Call the resume button when the button is clicked
        button.onClick.AddListener(() => 
        {
            Callbacks.Resume(SourceUI);
        });
    }
}
