using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CampaignLevelButton : MonoBehaviour
{
    private TextMeshProUGUI text;

    private int level;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            if (text == null)
            {
                GetComponent<Button>().onClick.AddListener(OnClick);
                text = GetComponentInChildren<TextMeshProUGUI>();
            }
            //Update the level text based on the level number set
            text.text = "Level " + (level + 1);
        }
    }

    private void OnClick()
    {
        //When the button is clicked, play the campaign level
        Callbacks.PlayCampaignLevel(Level);
    }
}
