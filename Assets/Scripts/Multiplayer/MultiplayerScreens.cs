using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;


public class MultiplayerScreens : MonoBehaviour
{
    [Tooltip("The objects that make up a player's screen. These objects will be duplicated for each player in the game")]
    [SerializeField] PlayerScreen BaseScreenObjects; //The screen that all the other screens will copy from
    private static Dictionary<int, PlayerScreen> OtherPlayerScreens = new Dictionary<int, PlayerScreen>(); //A list of all the other player screens
    public static int PlayersAdded { get; private set; } = 1; //How many player screens have been added
    private static MultiplayerScreens Singleton; //The singleton for the multiplayer screens

    public static PlayerScreen Primary => GetPlayerScreen(1); //The primary UI screen

    private void Start()
    {
        //Set the singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    //Called when the level is unloaded
    [RuntimeInitializeOnLoadMethod]
    private static void UnloadHandler()
    {
        GameManager.OnLevelUnload += () =>
        {
            DeletePlayerScreens();
            Primary.PlayerCamera.Target = null;
        };
    }

    static FieldInfo[] playerInfoFields; //The fields of the PlayerScreen Class

    //Adds a new screen for a player
    public static PlayerScreen AddPlayerScreen()
    {
        //Set the "Main" scene to be active
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        //Increase the player's added
        PlayersAdded++;
        if (playerInfoFields == null)
        {
            playerInfoFields = typeof(PlayerScreen).GetFields();
        }
        //Create the new player screen
        PlayerScreen screen = new PlayerScreen();
        //Copy the base components from the BaseScreenObjects to the newly created one
        foreach (var field in playerInfoFields)
        {
            var original = field.GetValue(Singleton.BaseScreenObjects) as Component;
            Object copy = Instantiate(original.gameObject).GetComponent(field.FieldType);
            if (copy is IPlayerSpecific psCopy)
            {
                psCopy.PlayerNumber = PlayersAdded;
            }
            field.SetValue(screen, copy);
        }
        //Notify the other screens that a new screen has been added
        for (int i = 1; i < PlayersAdded; i++)
            {
            var otherSpecifics = GetPlayerScreen(i);
            foreach (var field in playerInfoFields)
            {
                var value = field.GetValue(otherSpecifics);
                if (value is PlayerSpecific ps)
                {
                    ps.OnNewPlayerChange();
                }
            }
        }
        //Add the newly created screen to the list of player screens
        OtherPlayerScreens.Add(PlayersAdded, screen);
        //Set the active screen back to the game scene
        SceneManager.SetActiveScene(activeScene);
        return screen;
    }

    //Get a screen for a specific player
    public static PlayerScreen GetPlayerScreen(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return Singleton.BaseScreenObjects;
        }
        else
        {
            return OtherPlayerScreens.TryGetValue(playerNumber, out var r) ? r : throw new Exception($"There is no screen for player {playerNumber}");
        }
    }

    //Get an iterator to iterate over all the screens
    public static IEnumerable<PlayerScreen> GetAllScreens()
    {
        for (int i = 1; i <= PlayersAdded; i++)
        {
            yield return GetPlayerScreen(i);
        }
    }

    //Delete all of the other player screens
    public static void DeletePlayerScreens()
    {
        if (PlayersAdded > 1)
        {
            for (int i = PlayersAdded; i > 1; i--)
            {
                var specifics = OtherPlayerScreens[i];
                OtherPlayerScreens.Remove(i);
                foreach (var field in playerInfoFields)
                {
                    var value = field.GetValue(specifics) as Component;
                    Destroy(value.gameObject);
                }
            }
            PlayersAdded = 1;
            //Notify the first screen that the other screens have been deleted
            foreach (var otherSpecifics in GetAllScreens())
            {
                foreach (var field in playerInfoFields)
                {
                    var value = field.GetValue(otherSpecifics);
                    if (value is PlayerSpecific ps)
                    {
                        ps.OnNewPlayerChange();
                    }
                }
            }
        }
    }
}
