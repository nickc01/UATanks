using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class GameManager
{
    [Header("Levels")]
    [Tooltip("How many levels to use in the campaign")]
    public int Levels = 3;

    public static class UI
    {
        //Called when the play button is pressed
        //Used to start the game
        public static void Play(LevelLoadMode loadMode)
        {
            //Show the game UI
            UIManager.SetUIState("Game");
            //Load the game scene
            Game.StartCoroutine(LoadGameScene(loadMode));
        }

        public static void GoToCampaign()
        {
            UIManager.SetUIState("Campaign");
        }

        public static void ToModeSelectScreen()
        {
            UIManager.SetUIState("SinglePlayerMode");
        }

        public static void ToMainMenu()
        {
            UIManager.SetUIState("Main Menu");
        }
    }
}
