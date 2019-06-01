﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    private static bool playingLevel = false; //Whether the player is playing a level or not
    public static bool PlayingLevel //The public interface for whether the player is playing a level
    {
        get => playingLevel;
        set
        {
            if (playingLevel != value)
            {
                playingLevel = value;
                PlayingLevelEvent?.Invoke(value);
            }
        }
    }
    public static event Action<bool> PlayingLevelEvent; //An event that is called whenever the game is playing or not
    public static LevelLoadMode CurrentLoadMode { get; private set; } = LevelLoadMode.Random; //The mode the map generator will use
    public static int LevelSeed { get; set; } = 0; //The current campaign level loaded
    public static GameManager Game { get; private set; } //The singleton for the game manager
    //public static (PlayerTank Tank,TankData Data) Player; //The data of the current player in the game
    public static List<(PlayerTank Tank, TankData Data)> Players = new List<(PlayerTank Tank, TankData Data)>();
    public static List<(EnemyTank Tank,TankData Data)> Enemies = new List<(EnemyTank,TankData)>(); //The data of all the enemies in the game
    public static Dictionary<Type, List<PowerupHolder>> AllPowerups = new Dictionary<Type, List<PowerupHolder>>(); //All the powerups spawned in the game, sorted by powerup type

    [Header("Prefabs")]
    [Tooltip("The prefab used whenever a tank fires a shell")]
    public GameObject ShellPrefab;
    [Tooltip("The player prefab")]
    public GameObject PlayerPrefab;
    [Tooltip("The bomb prefab used when the bomb powerup is picked up")]
    public GameObject BombPrefab;
    [Tooltip("The prefab used when a tank dies, or when a bomb powerup is used")]
    public GameObject ExplosionPrefab;
    [Tooltip("A list of possible enemies to spawn at the enemy spawnpoints")]
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    [Tooltip("A list of possible powerups to spawn at the powerup spawnpoints")]
    public List<PowerupHolder> PowerUps = new List<PowerupHolder>();

    [Header("Sounds")]
    [Tooltip("Played When a shell is fired")]
    public List<AudioClip> FireSounds;
    [Tooltip("Played when you win the game")]
    public AudioClip WinSound;
    [Tooltip("Played when you lose the game")]
    public AudioClip LoseSound;
    [Tooltip("The music that is played on the main menu")]
    public AudioClip MenuMusic;
    [Tooltip("Music Tracks that are played during the course of the game")]
    public List<AudioClip> GameMusic;
    [Tooltip("Sounds that are played when a tank dies or when a bomb powerup is used")]
    public List<AudioClip> ExplosionSounds;
    [Tooltip("Sound that is played when a shell collides with a tank")]
    public List<AudioClip> ShellHitSounds;
    [Tooltip("Sound that is played when a powerup is collected")]
    public AudioClip PowerupPickupSound;
    [Tooltip("Sounds that are played when a menu button is clicked")]
    public List<AudioClip> ButtonClickSounds;
    [Tooltip("A looping sound effect that is played when a tank moves")]
    public AudioClip TankMoveSound;

    [Header("Shield")]
    [Space]
    [Header("Powerup Stats")]
    [Tooltip("How much damage the shield can withstand")]
    public float ShieldStrength = 10f;
    [Tooltip("How fast the warning notifier will flash. Number's in flashes per second")]
    public float ShieldWarningFlashRate = 20f;
    [Tooltip("How much damage resistance the shield will apply to the tank")]
    public float ShieldDamageResistance = 10f;
    
    [Header("Health")]
    [Tooltip("How much health the health powerup will restore on the tank")]
    public float HealthRestoreAmount = 50f;

    [Header("Speed")]
    [Tooltip("How fast the tank will go when the speed powerup is collected")]
    public float PowerupSpeed = 10f;

    [Header("Bomb")]
    [Tooltip("How many bombs will be dropped per second")]
    public float BombRate = 1f;
    [Tooltip("How long each bomb will take to detonate")]
    public float BombTime;
    [Tooltip("How large the bomb explosion will be")]
    public float BombExplosionSize;
    [Tooltip("How much damage the explosion will do to tanks")]
    public float BombDamage;

    [Header("Audio")]
    [Tooltip("The Main Audio Group")]
    public AudioMixer MainAudio;

    [Header("Control Schemes")]
    [Tooltip("The control scheme for player 1")]
    public ControlScheme Player1Scheme;
    [Tooltip("The control scheme for player 2")]
    public ControlScheme Player2Scheme;

    private void Start()
    {
        //Set the singleton
        if (Game == null)
        {
            Game = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //If the game scene is already active
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            UI.Play(LevelLoadMode.Random);
        }
    }

    public static (PlayerTank Tank, TankData Data) GetNearestPlayer(Vector3 Position)
    {
        (float distance, PlayerTank tank) result = (float.PositiveInfinity, null);
        foreach (var player in Players)
        {
            var distance = Vector3.Distance(player.Tank.transform.position, Position);
            if (distance <= result.distance)
            {
                result = (distance, player.Tank);
            }
        }
        return (result.tank, result.tank?.Data);
    }

    public static (PlayerTank Tank, TankData Data) GetNearestPlayer(Transform transform)
    {
        return GetNearestPlayer(transform.position);
    }

    public static (PlayerTank Tank, TankData Data) GetNearestPlayer(GameObject gameObject)
    {
        return GetNearestPlayer(gameObject.transform.position);
    }

    public static (PlayerTank Tank, TankData Data) GetNearestPlayer<T>(T component) where T : Component
    {
        return GetNearestPlayer(component.gameObject);
    }

    //A function to quit the game
    public static void Quit()
    {
        //Quit the game
        Application.Quit();
    }

    //Called when all the enemy tanks in the map have been destroyed
    public static void Win(PlayerTank Winner,bool PlayWinSound = true)
    {
        //Show the win screen
        //UIManager.SetUIStateAll("Win",Curves.Smooth,FromIsHidden: true);
        //var UI = MultiplayerManager.GetPlayerInfo(Winner.PlayerNumber).PlayerUI;
        Winner.UI.ButtonsEnabled = true;
        Winner.UI.SetUIState("Win", Curves.Smooth, FromIsHidden: true);
        //Play the Win Sound
        //CameraController.Main.Sound.clip = Game.WinSound;
        //CameraController.Main.Sound.Play();
        if (PlayWinSound)
        {
            AudioPlayer.Play(Game.WinSound, Audio.SoundEffects, Vector3.zero, Vector3.zero);
        }
        PlayingLevel = false;
        Winner.UI.ResultsScore = Winner.Score;
        //MultiplayerManager.DeletePlayers();
    }

    //Called when the player tank has been destroyed
    public static void Lose(PlayerTank Loser,bool PlayLoseSound = true)
    {
        //var UI = MultiplayerManager.GetPlayerInfo(Loser.PlayerNumber).PlayerUI;
        //var UI = Loser.UI;
        if (Players.Count == 0)
        {
            Loser.UI.ButtonsEnabled = true;
            PlayingLevel = false;
        }
        else
        {
            Loser.UI.ButtonsEnabled = false;
        }
        if (PlayLoseSound)
        {
            AudioPlayer.Play(Game.LoseSound, Audio.SoundEffects,Vector3.zero,Vector3.zero);
        }
        Loser.UI.SetUIState("Lose", Curves.Smooth, FromIsHidden: true);
        Loser.UI.ResultsScore = Loser.Score;
    }

    //A routine to unload the level
    public static IEnumerator UnloadLevel()
    {
        //If the game is loaded
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            MultiplayerScreens.DeletePlayerScreens();
            UIManager.Primary.Gradient = false;
            //Unload it
            yield return SceneManager.UnloadSceneAsync("Game");
            AudioPlayer.Listeners.Clear();
        }
    }

    //A routine to load the level and play the game
    static IEnumerator LoadGameScene(LevelLoadMode loadMode)
    {
        //Reset all the tank stats
        //Player = (null, null);
        Players.Clear();
        Enemies.Clear();
        Tank.AllTanks.Clear();
        //Set the seed of the map generator depending on the level load mode
        CurrentLoadMode = loadMode;
        switch (loadMode)
        {
            case LevelLoadMode.Specific:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.UseSeed;
                MapGenerator.Generator.Seed = LevelSeed;
                break;
            case LevelLoadMode.MapOfTheDay:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.MapOfTheDay;
                break;
            case LevelLoadMode.Random:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.Random;
                break;
        }
        //If the game scene is not loaded yet
        if (!SceneManager.GetSceneByName("Game").isLoaded)
        {
            //Load the game scene
            yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        }
        //Set the scene active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        //Generate the map
        MapGenerator.Generator.GenerateMap(loadMode == LevelLoadMode.Specific ? LevelSeed : 0);
        //Spawn the player at a random spawnpoint
        //var spawnPoint = MapGenerator.Generator.PopPlayerSpawnPoint();
        //Instantiate(Game.PlayerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        PlayerTank.Create(MapGenerator.Generator.PopPlayerSpawnPoint().transform.position, 1,false);
        //If there are two players playing
        if (Options.PlayerCount.value == 1)
        {
            PlayerTank.Create(MapGenerator.Generator.PopPlayerSpawnPoint().transform.position, 2);
        }
        /*foreach (var info in MultiplayerManager.GetAllPlayerInfo())
        {
            info.PlayerUI.Gradient = true;
        }*/
        foreach (var screen in MultiplayerScreens.GetAllScreens())
        {
            screen.PlayerUI.Gradient = true;
        }
        yield return UI.ShowReadySequence();
        //Show the game UI for all screens
        UIManager.All.SetUIState("Game");
        //Set the playing level flag
        PlayingLevel = true;
    }

}
