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

    //Called when the play button is pressed
    public static void SinglePlayer()
    {
        GameManager.UI.ToModeSelectScreen();
    }

    public static void MainMenu()
    {
        CoroutineManager.StartCoroutine(MainMenuRoutine());
    }

    public static void NextLevel()
    {
        CoroutineManager.StartCoroutine(NextLevelRoutine());
    }

    public static void Help()
    {
        GameManager.UI.ToHelpScreen();
    }

    public static void Quit()
    {
        GameManager.Quit();
    }

    static IEnumerator NextLevelRoutine()
    {
        yield return GameManager.UnloadLevel();
        PlayCampaignLevel(GameManager.CurrentCampaignLevel + 1);
        //GameManager.CurrentCampaignLevel++;
        //GameManager.UI.Play(LevelLoadMode.Campaign);
    }

    static IEnumerator MainMenuRoutine()
    {
        yield return GameManager.UnloadLevel();
        GameManager.UI.ToMainMenu();
    }


    #endregion

    #region Single Player Mode Select

    public static void Campaign()
    {
        //GameManager.UI.Play(LevelLoadMode.Campaign);
        GameManager.UI.GoToCampaign();
    }

    public static void PlayCampaignLevel(int levelNumber)
    {
        GameManager.CurrentCampaignLevel = levelNumber;
        MapGenerator.Generator.MapHeight = Mathf.FloorToInt(levelNumber / 2f) + 2;
        MapGenerator.Generator.MapWidth = Mathf.FloorToInt(levelNumber / 2f) + 1;
        GameManager.UI.Play(LevelLoadMode.Campaign);
    }

    public static void Map_Day()
    {
        MapGenerator.Generator.MapHeight = 5;
        MapGenerator.Generator.MapWidth = 5;
        GameManager.UI.Play(LevelLoadMode.MapOfTheDay);
    }

    public static void RandomLevel()
    {
        MapGenerator.Generator.MapHeight = 5;
        MapGenerator.Generator.MapWidth = 5;
        GameManager.UI.Play(LevelLoadMode.Random);
    }

    #endregion

    #region Help Area

    #endregion
}
