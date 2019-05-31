using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    Button button;
    //Enables the button if there are more levels left to play
    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }
        button.enabled = GameManager.LevelSeed + 1 < GameManager.Game.Levels && GameManager.CurrentLoadMode == LevelLoadMode.Specific;
    }
}
