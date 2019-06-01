using System;
using System.Collections;
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
            //Load the game scene
            Game.StartCoroutine(LoadGameScene(loadMode));
        }

        public static void Restart()
        {
            CoroutineManager.StartCoroutine(RestartRoutine());
        }

        private static IEnumerator RestartRoutine()
        {
            yield return UnloadLevel();
            LevelSeed = MapGenerator.Generator.Seed;
            Play(LevelLoadMode.Specific);
        }

        public static void GoToOptions()
        {
            UIManager.All.SetUIState("Options", Curves.Smooth, TransitionMode.TopToBottom);
        }

        //The routine for showing the ready sequence at the beginning of each level
        public static IEnumerator ShowReadySequence()
        {
            UIManager.All.SetUIState("Ready?");
            yield return new WaitForSeconds(1.5f);
            UIManager.All.SetUIState("Ready3",Curves.ReadyCurve,TransitionMode.TopToBottom,1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.All.SetUIState("Ready2", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.All.SetUIState("Ready1", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.All.SetUIState("Go!", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
        }

        //A function to go to the help screen
        public static void ToHelpScreen()
        {
            UIManager.All.SetUIState("Help", Curves.Smooth, TransitionMode.TopToBottom);
        }
        //A function to go to the campaign screen
        public static void GoToCampaign()
        {
            UIManager.All.SetUIState("Campaign",Curves.Smooth,TransitionMode.TopToBottom);
        }
        //A function to go to the mode select screen
        public static void ToModeSelectScreen()
        {
            UIManager.All.SetUIState("SinglePlayerMode", Curves.Smooth, TransitionMode.TopToBottom);
        }
        //A function to go to the main menu
        public static void ToMainMenu()
        {
            CoroutineManager.StartCoroutine(ToMainMenuRoutine());
            //UIManager.All.SetUIState("Main Menu", Curves.Smooth, TransitionMode.BottomToTop);
        }

        static IEnumerator ToMainMenuRoutine()
        {
            yield return UnloadLevel();
            UIManager.All.SetUIState("Main Menu", Curves.Smooth, TransitionMode.BottomToTop);
        }
    }
}
