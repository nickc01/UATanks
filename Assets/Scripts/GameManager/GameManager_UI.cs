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
            LevelSeed = MapGenerator.Generator.Seed;
            Play(LevelLoadMode.Specific);
        }

        public static void GoToOptions()
        {
            UIManager.All.SetUIState("Options", Curves.Smoothest, TransitionMode.TopToBottom);
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
            UIManager.All.SetUIState("Help", Curves.Smoothest, TransitionMode.TopToBottom);
        }
        //A function to go to the main menu
        public static void ToMainMenu()
        {
            CoroutineManager.StartCoroutine(ToMainMenuRoutine());
        }

        static IEnumerator ToMainMenuRoutine()
        {
            yield return UnloadLevel(MusicType.Menu);
            UIManager.All.SetUIState("Main Menu", Curves.Smoothest, TransitionMode.BottomToTop);
        }
    }
}
