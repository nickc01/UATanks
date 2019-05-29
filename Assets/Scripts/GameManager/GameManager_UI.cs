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

        public static void GoToOptions()
        {
            UIManager.SetUIStateAll("Options", Curves.Smooth, TransitionMode.TopToBottom);
        }

        //The routine for showing the ready sequence at the beginning of each level
        public static IEnumerator ShowReadySequence()
        {
            UIManager.SetUIStateAll("Ready?");
            yield return new WaitForSeconds(1.5f);
            UIManager.SetUIStateAll("Ready3",Curves.ReadyCurve,TransitionMode.TopToBottom,1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.SetUIStateAll("Ready2", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.SetUIStateAll("Ready1", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
            UIManager.SetUIStateAll("Go!", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.2f);
        }

        //A function to go to the help screen
        public static void ToHelpScreen()
        {
            UIManager.SetUIStateAll("Help", Curves.Smooth, TransitionMode.TopToBottom);
        }
        //A function to go to the campaign screen
        public static void GoToCampaign()
        {
            UIManager.SetUIStateAll("Campaign",Curves.Smooth,TransitionMode.TopToBottom);
        }
        //A function to go to the mode select screen
        public static void ToModeSelectScreen()
        {
            UIManager.SetUIStateAll("SinglePlayerMode", Curves.Smooth, TransitionMode.TopToBottom);
        }
        //A function to go to the main menu
        public static void ToMainMenu()
        {
            UIManager.SetUIStateAll("Main Menu", Curves.Smooth, TransitionMode.BottomToTop);
        }
    }
}
