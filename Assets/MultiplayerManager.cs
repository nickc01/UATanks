using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] PlayerSpecifics BasePlayerSpecifics;
    //[SerializeField] List<GameObject> OtherBaseSpecifics;

    //private static Dictionary<int, List<GameObject>> OtherPlayerClones = new Dictionary<int, List<GameObject>>();
    private static Dictionary<int, PlayerSpecifics> PlayerClones = new Dictionary<int, PlayerSpecifics>();

    public static int PlayersAdded { get; private set; } = 1;

    private static MultiplayerManager Singleton;

    //public static event Action AddedPlayersUpdate;

    public static PlayerSpecifics Primary => GetPlayerSpecifics(1);

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

    static FieldInfo[] playerSpecificFields;

    public static PlayerSpecifics CreateNewPlayerSpecifics()
    {
        PlayersAdded++;
        if (playerSpecificFields == null)
        {
            playerSpecificFields = typeof(PlayerSpecifics).GetFields();
        }
        PlayerSpecifics specifics = new PlayerSpecifics();
        foreach (var field in playerSpecificFields)
        {
            Object copy = null;// = Instantiate(field.GetValue(Singleton.BasePlayerSpecifics) as Object);
            var original = field.GetValue(Singleton.BasePlayerSpecifics) as Component;
            if (original is IPlayerSpecificInstantiation duper)
            {
                copy = duper.DupeObject(original);
            }
            else
            {
                copy = Instantiate(original.gameObject).GetComponent(field.FieldType);
            }
            if (copy is IPlayerSpecific psCopy)
            {
                psCopy.PlayerID = PlayersAdded;
            }
            field.SetValue(specifics, copy);
        }
        //foreach (var otherSpecifics in GetAllSpecifics())
        //{
        for (int i = 1; i < PlayersAdded; i++)
            {
            var otherSpecifics = GetPlayerSpecifics(i);
            foreach (var field in playerSpecificFields)
            {
                var value = field.GetValue(otherSpecifics);
                if (value is PlayerSpecific ps)
                {
                    ps.OnNewPlayerChange();
                }
            }
        }
        //}
        PlayerClones.Add(PlayersAdded, specifics);
        //AddedPlayersUpdate?.Invoke();
        return specifics;
    }

    public static PlayerSpecifics GetPlayerSpecifics(int playerID)
    {
        if (playerID == 1)
        {
            return Singleton.BasePlayerSpecifics;
        }
        else
        {
            return PlayerClones.TryGetValue(playerID, out var r) ? r : throw new Exception($"There is no player {playerID} ");
        }
    }

    public static IEnumerable<PlayerSpecifics> GetAllSpecifics()
    {
        for (int i = 1; i <= PlayersAdded; i++)
        {
            yield return GetPlayerSpecifics(i);
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
                foreach (var field in playerSpecificFields)
                {
                    var value = field.GetValue(specifics) as Object;
                    if (value is IPlayerSpecificInstantiation destroyer)
                    {
                        destroyer.DestroyObject(value);
                    }
                    else
                    {
                        Destroy(value);
                    }
                }
            }
            PlayersAdded = 1;
            foreach (var otherSpecifics in GetAllSpecifics())
            {
                foreach (var field in playerSpecificFields)
                {
                    var value = field.GetValue(otherSpecifics);
                    if (value is PlayerSpecific ps)
                    {
                        ps.OnNewPlayerChange();
                    }
                }
            }
            //AddedPlayersUpdate?.Invoke();
        }
    }
}
