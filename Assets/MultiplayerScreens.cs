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
    [SerializeField] PlayerScreen BaseScreenObjects;
    private static Dictionary<int, PlayerScreen> OtherPlayerScreens = new Dictionary<int, PlayerScreen>();
    public static int PlayersAdded { get; private set; } = 1;
    private static MultiplayerScreens Singleton;

    public static PlayerScreen Primary => GetPlayerScreen(1);

    private void Start()
    {
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

    static FieldInfo[] playerInfoFields;

    public static PlayerScreen AddPlayerScreen()
    {
        var activeScene = SceneManager.GetActiveScene();
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
        PlayersAdded++;
        if (playerInfoFields == null)
        {
            playerInfoFields = typeof(PlayerScreen).GetFields();
        }
        PlayerScreen screen = new PlayerScreen();
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
        OtherPlayerScreens.Add(PlayersAdded, screen);
        SceneManager.SetActiveScene(activeScene);
        return screen;
    }

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

    public static IEnumerable<PlayerScreen> GetAllScreens()
    {
        for (int i = 1; i <= PlayersAdded; i++)
        {
            yield return GetPlayerScreen(i);
        }
    }

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
