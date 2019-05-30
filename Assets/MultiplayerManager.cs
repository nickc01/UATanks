using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] PlayerInfo BasePlayerInfo;
    private static Dictionary<int, PlayerInfo> PlayerClones = new Dictionary<int, PlayerInfo>();
    public static int PlayersAdded { get; private set; } = 1;
    private static MultiplayerManager Singleton;

    public static PlayerInfo Primary => GetPlayerInfo(1);

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

    public static PlayerInfo AddNewPlayer()
    {
        PlayersAdded++;
        if (playerInfoFields == null)
        {
            playerInfoFields = typeof(PlayerInfo).GetFields();
        }
        PlayerInfo info = new PlayerInfo();
        foreach (var field in playerInfoFields)
        {
            var original = field.GetValue(Singleton.BasePlayerInfo) as Component;
            Object copy = Instantiate(original.gameObject).GetComponent(field.FieldType);
            if (copy is IPlayerSpecific psCopy)
            {
                psCopy.PlayerNumber = PlayersAdded;
            }
            field.SetValue(info, copy);
        }
        for (int i = 1; i < PlayersAdded; i++)
            {
            var otherSpecifics = GetPlayerInfo(i);
            foreach (var field in playerInfoFields)
            {
                var value = field.GetValue(otherSpecifics);
                if (value is PlayerSpecific ps)
                {
                    ps.OnNewPlayerChange();
                }
            }
        }
        PlayerClones.Add(PlayersAdded, info);
        return info;
    }

    public static PlayerInfo GetPlayerInfo(int playerNumber)
    {
        if (playerNumber == 1)
        {
            return Singleton.BasePlayerInfo;
        }
        else
        {
            return PlayerClones.TryGetValue(playerNumber, out var r) ? r : throw new Exception($"There is no player {playerNumber} ");
        }
    }

    public static IEnumerable<PlayerInfo> GetAllPlayerInfo()
    {
        for (int i = 1; i <= PlayersAdded; i++)
        {
            yield return GetPlayerInfo(i);
        }
    }

    public static void DeletePlayers()
    {
        if (PlayersAdded > 1)
        {
            for (int i = PlayersAdded; i > 1; i--)
            {
                var specifics = PlayerClones[i];
                PlayerClones.Remove(i);
                foreach (var field in playerInfoFields)
                {
                    var value = field.GetValue(specifics) as Object;
                    Destroy(value);
                }
            }
            PlayersAdded = 1;
            foreach (var otherSpecifics in GetAllPlayerInfo())
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
