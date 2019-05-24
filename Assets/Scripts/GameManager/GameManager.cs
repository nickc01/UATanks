using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class GameManager : MonoBehaviour
{
    private static bool playingLevel = false;
    public static bool PlayingLevel
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
    public static event Action<bool> PlayingLevelEvent;
    public static LevelLoadMode CurrentLoadMode { get; private set; } = LevelLoadMode.None;
    public static int CurrentCampaignLevel { get; set; } = 0;
    public static GameManager Game { get; private set; } //The singleton for the game manager
    public static (PlayerTank Tank,TankData Data) Player; //The data of the current player in the game
    public static List<(EnemyTank Tank,TankData Data)> Enemies = new List<(EnemyTank,TankData)>(); //The data of all the enemies in the game

    [Header("Prefabs")]
    [Tooltip("The prefab used whenever a tank fires a shell")]
    public GameObject ShellPrefab;
    [Tooltip("The player prefab")]
    public GameObject PlayerPrefab;
    [Tooltip("The bomb prefab used when the bomb powerup is picked up")]
    public GameObject BombPrefab;
    [Tooltip("The prefab used when the bomb explodes")]
    public GameObject ExplosionPrefab;
    [Tooltip("A list of possible enemies to spawn at the enemy spawnpoints")]
    public List<GameObject> EnemyPrefabs = new List<GameObject>();
    [Tooltip("A list of possible powerups to spawn at the powerup spawnpoints")]
    public List<PowerupHolder> PowerUps = new List<PowerupHolder>();

    [Header("Sounds")]
    [Tooltip("Played When a shell is fired")]
    public AudioClip FireSound;
    [Tooltip("Played when you win the game")]
    public AudioClip WinSound;
    [Tooltip("Played when you lose the game")]
    public AudioClip LoseSound;

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
            UI.Play(LevelLoadMode.None);
        }
    }

    public static void Quit()
    {
        Application.Quit();
    }

    //Called when all the enemy tanks in the map have been destroyed
    public static void Win()
    {
        //Show the win screen
        UIManager.SetUIState("Win",Curves.Smooth,FromIsHidden: true);
        //Play the Win Sound
        CameraController.Main.Sound.clip = Game.WinSound;
        CameraController.Main.Sound.Play();
        PlayingLevel = false;
    }

    //Called when the player tank has been destroyed
    public static void Lose()
    {
        //Show the lose screen
        UIManager.SetUIState("Lose", Curves.Smooth, FromIsHidden: true);
        //Play the Lose Sound
        CameraController.Main.Sound.clip = Game.LoseSound;
        CameraController.Main.Sound.Play();
        PlayingLevel = false;
    }

    public static IEnumerator UnloadLevel()
    {
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync("Game");
        }
    }

    static IEnumerator LoadGameScene(LevelLoadMode loadMode)
    {
        Player = (null, null);
        Enemies.Clear();
        Controller.AllTanks.Clear();
        CurrentLoadMode = loadMode;
        switch (loadMode)
        {
            case LevelLoadMode.None:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.Random;
                break;
            case LevelLoadMode.Campaign:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.UseSeed;
                MapGenerator.Generator.Seed = CurrentCampaignLevel;
                break;
            case LevelLoadMode.MapOfTheDay:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.MapOfTheDay;
                break;
            case LevelLoadMode.Random:
                MapGenerator.Generator.SeedGenerator = SeedGenerator.Random;
                break;
        }
        if (!SceneManager.GetSceneByName("Game").isLoaded)
        {
            //Load the game scene
            yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        }
        //Set the scene active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        //Generate the map
        MapGenerator.Generator.GenerateMap();
        //Spawn the player at a random spawnpoint
        var spawnPoint = MapGenerator.Generator.PopPlayerSpawnPoint();
        Instantiate(Game.PlayerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        yield return UI.ShowReadySequence();
        //Show the game UI
        UIManager.SetUIState("Game");
        PlayingLevel = true;
    }

}
