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

        public static IEnumerator ShowReadySequence()
        {
            UIManager.SetUIState("Ready?");
            yield return new WaitForSeconds(2f);
            UIManager.SetUIState("Ready3",Curves.ReadyCurve,TransitionMode.TopToBottom,1f);
            yield return new WaitForSeconds(1.5f);
            UIManager.SetUIState("Ready2", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.5f);
            UIManager.SetUIState("Ready1", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.5f);
            UIManager.SetUIState("Go!", Curves.ReadyCurve, TransitionMode.TopToBottom, 1f);
            yield return new WaitForSeconds(1.5f);
        }

        public static void ToHelpScreen()
        {
            UIManager.SetUIState("Help", Curves.Smooth, TransitionMode.TopToBottom);
        }

        public static void GoToCampaign()
        {
            UIManager.SetUIState("Campaign",Curves.Smooth,TransitionMode.TopToBottom);
        }

        public static void ToModeSelectScreen()
        {
            UIManager.SetUIState("SinglePlayerMode", Curves.Smooth, TransitionMode.TopToBottom);
        }

        public static void ToMainMenu()
        {
            UIManager.SetUIState("Main Menu", Curves.Smooth, TransitionMode.BottomToTop);
        }
    }
}
