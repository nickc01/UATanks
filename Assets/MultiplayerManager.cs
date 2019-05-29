using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public struct PlayerSpecifics
{
    public UIManager UIManager;
    public CameraController Camera;
}

public interface IIsPlayerSpecific
{
    int PlayerID { get; set; }
}


public class MultiplayerManager : MonoBehaviour
{
    [SerializeField] PlayerSpecifics BasePlayerSpecifics;
    [SerializeField] List<GameObject> OtherBaseSpecifics;

    private static Dictionary<int, List<GameObject>> OtherPlayerClones = new Dictionary<int, List<GameObject>>();
    private static Dictionary<int, PlayerSpecifics> PlayerClones = new Dictionary<int, PlayerSpecifics>();

    public static int PlayersAdded { get; private set; } = 1;

    private static MultiplayerManager Singleton;

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
        foreach (var baseObj in OtherBaseSpecifics)
        {
            foreach (var playerSpecific in baseObj.GetComponentsInChildren<IIsPlayerSpecific>())
            {
                playerSpecific.PlayerID = 1;
            }
        }
    }

    public static void CreateNewPlayerSpecifics()
    {
        PlayersAdded++;
        PlayerSpecifics specifics;
        specifics.Camera = Instantiate(Singleton.BasePlayerSpecifics.Camera);
        specifics.Camera.PlayerID = PlayersAdded;
        specifics.UIManager = Instantiate(Singleton.BasePlayerSpecifics.UIManager);
        specifics.UIManager.PlayerID = PlayersAdded;
        var NewPlayerSpecifics = new List<GameObject>(Singleton.OtherBaseSpecifics.Capacity);
        foreach (var original in Singleton.OtherBaseSpecifics)
        {
            var clone = Instantiate(original);
            foreach (var playerSpecific in clone.GetComponentsInChildren<IIsPlayerSpecific>())
            {
                playerSpecific.PlayerID = PlayersAdded;
            }
            NewPlayerSpecifics.Add(clone);
        }
    }

    public static ReadOnlyCollection<GameObject> GetOtherPlayerSpecifics(int playerID)
    {
        return OtherPlayerClones.TryGetValue(playerID, out var result) ? result.AsReadOnly() : throw new Exception($"There is no player {playerID} ");
    }

    public static PlayerSpecifics GetPlayerSpecifics(int playerID)
    {
        return PlayerClones.TryGetValue(playerID, out var r) ? r : throw new Exception($"There is no player {playerID} ");
    }

    public static void DeletePlayer()
    {

    }
}
