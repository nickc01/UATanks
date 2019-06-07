using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    Button button; //The pause button object
    UIManager SourceUI; //The source UI

    private void Start()
    {
        //Get the components
        button = GetComponent<Button>();
        SourceUI = GetComponentInParent<UIManager>();
        //Call the pause function when the button is clicked
        button.onClick.AddListener(() =>
        {
            Callbacks.Pause(SourceUI);
        });
    }
}
