using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//A list of all the button callbacks in the game
public static class Callbacks
{
    //Called when the single player button is pressed
    //Plays either a random map or the map of the day depending on the dropdown selection
    public static void PlayGame()
    {
        switch (Options.MapType.Value)
        {
            case MapType.Random:
                GameManager.UI.Play(LevelLoadMode.Random);
                break;
            case MapType.MapOfTheDay:
                GameManager.UI.Play(LevelLoadMode.MapOfTheDay);
                break;
        }
    }

    //Called when the option button is pressed
    public static void OptionsButton()
    {
        GameManager.UI.GoToOptions();
    }

    //Called when any main menu button is pressed
    public static void MainMenu()
    {
        GameManager.UI.ToMainMenu();
    }

    //Called when the restart button is pressed
    public static void Restart()
    {
        GameManager.UI.Restart();
    }

    //Called when the pause button is pressed
    public static void Pause(UIManager sourceUI)
    {
        //Pause the game and show the pause screen on the source UI
        GameManager.SetPausedState(true,sourceUI);
    }

    //Called when the resume button is pressed
    public static void Resume(UIManager sourceUI)
    {
        //Unpause the game and hide the pause screen on the source UI
        GameManager.SetPausedState(false, sourceUI);
    }

    //Called when the help button is pressed
    public static void Help()
    {
        //Go to the help screen
        GameManager.UI.ToHelpScreen();
    }

    //Called when the quit button is pressed
    public static void Quit()
    {
        Debug.Log("Quitting Game");
        //Quit the game
        Application.Quit();
    }
}
