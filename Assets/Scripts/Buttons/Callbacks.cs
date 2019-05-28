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
    #region Main Menu

    //Called when the single player button is pressed
    public static void PlayGame()
    {
        //GameManager.UI.ToModeSelectScreen();
        var dropdownValue = Options.MapType.value;
        if (dropdownValue == 0)
        {
            Map_Day();
        }
        else
        {
            RandomLevel();
        }
    }

    public static void OptionsButton()
    {
        GameManager.UI.GoToOptions();
    }

    //Called when any main menu button is pressed
    public static void MainMenu()
    {
        //Start the main menu routine
        CoroutineManager.StartCoroutine(MainMenuRoutine());
    }

    //Called when the next level button is pressed
    public static void NextLevel()
    {
        //Start the next level routine
        CoroutineManager.StartCoroutine(NextLevelRoutine());
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
        //Quit the game
        GameManager.Quit();
    }

    //The next level routine
    static IEnumerator NextLevelRoutine()
    {
        //Unload the current level
        yield return GameManager.UnloadLevel();
        //Play the next level
        PlayCampaignLevel(GameManager.CurrentCampaignLevel + 1);
        //GameManager.CurrentCampaignLevel++;
        //GameManager.UI.Play(LevelLoadMode.Campaign);
    }

    //The main menu routine
    static IEnumerator MainMenuRoutine()
    {
        //Unload the current level
        yield return GameManager.UnloadLevel();
        //Go to the main menu
        GameManager.UI.ToMainMenu();
    }


    #endregion

    #region Single Player Mode Select

    //Called when the campaign button is pressed
    public static void Campaign()
    {
        //Go to the campaign screen
        GameManager.UI.GoToCampaign();
    }

    //Called to play a selected campaign level
    public static void PlayCampaignLevel(int levelNumber)
    {
        //Set the campaign level number
        GameManager.CurrentCampaignLevel = levelNumber;
        //Play the selected campaign level
        GameManager.UI.Play(LevelLoadMode.Campaign);
    }

    //Called when the map of the day button is pressed
    public static void Map_Day()
    {
        //Reset the map's width and height
        MapGenerator.Generator.MapHeight = 5;
        MapGenerator.Generator.MapWidth = 5;
        //Play the map of the day
        GameManager.UI.Play(LevelLoadMode.MapOfTheDay);
    }



    //Called when the random button is pressed
    public static void RandomLevel()
    {
        //Reset the map's width and height
        MapGenerator.Generator.MapHeight = 5;
        MapGenerator.Generator.MapWidth = 5;
        //Play a random level
        GameManager.UI.Play(LevelLoadMode.Random);
    }

    #endregion
}
