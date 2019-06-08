using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public enum MusicType
{
    None, //Plays no music
    Menu, //Plays the main menu music
    Game //Plays the game music
}

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
    public static List<(PlayerTank Tank, TankData Data)> Players = new List<(PlayerTank Tank, TankData Data)>();
    public static List<(EnemyTank Tank,TankData Data)> Enemies = new List<(EnemyTank,TankData)>(); //The data of all the enemies in the game
    public static Dictionary<Type, List<PowerupHolder>> AllPowerups = new Dictionary<Type, List<PowerupHolder>>(); //All the powerups spawned in the game, sorted by powerup type

    public static event Action OnLevelUnload; //An event that is called when the level unloads
    public static event Action<bool> OnGamePause; //Called when the game is paused or unpaused

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

    [Header("Materials")]
    [Tooltip("The material for the obstacles and walls")]
    public Material ObstacleMaterial;
    [Tooltip("The material for the floor")]
    public Material FloorMaterial;

    [Header("Options")]
    [Tooltip("The main options area")]
    public Options MainOptions;

    private AudioObject MusicObject; //The audio object for the music

    private static Color currentColorInternal;
    public static Color CurrentGameColor //The current color of the game
    {
        get => currentColorInternal;
        set
        {
            currentColorInternal = value;
            Game.ObstacleMaterial.color = value;
        }
    }

    public static Color CurrentGameColorBright => Color.Lerp(Color.white, CurrentGameColor, 0.3f); //The current color but brighter
    public static Color CurrentGameColorDark => Color.Lerp(Color.black, CurrentGameColor, 0.3f); //The current color but darker
    public static float GameDT => Paused ? 0f : Time.deltaTime; //Similar to Time.deltaTime, but is zero when the game is paused

    private static UIManager PausedUI = null; //The UI showing the pause screen
    public static bool Paused { get; private set; } = false; //Whether the game is paused or not

    //Sets whether the game is paused or not
    public static void SetPausedState(bool value,UIManager screen)
    {
        if (value == true && Paused == false && PausedUI == null)
        {
            OnGamePause?.Invoke(true);
            Paused = true;
            PausedUI = screen;
            PausedUI.SetUIState("Paused", Curves.Smoothest, TransitionMode.TopToBottom, 2f);
        }
        if (value == false && Paused == true && PausedUI == screen)
        {
            OnGamePause?.Invoke(false);
            Paused = false;
            PausedUI.SetUIState("Game", Curves.Smoothest, TransitionMode.BottomToTop, 2f);
            PausedUI = null;
        }
    }


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
        //Set the current color of the game to a default color
        CurrentGameColor = new Color32(0, 162, 255, 255);
        //If the game scene is already active
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            //Play a random level
            UI.Play(LevelLoadMode.Random);
        }
        else
        {
            //Play the main menu music
            PlayMusic(MusicType.Menu);
            //Start the background panorama
            StartCoroutine(PanoramaGenerator.StartPanorama());
        }
    }

    //Gets the nearest player to a set position
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

    //Gets the nearest player to a transform
    public static (PlayerTank Tank, TankData Data) GetNearestPlayer(Transform transform)
    {
        return GetNearestPlayer(transform.position);
    }

    //Gets the nearest player to a gameobject
    public static (PlayerTank Tank, TankData Data) GetNearestPlayer(GameObject gameObject)
    {
        return GetNearestPlayer(gameObject.transform.position);
    }

    //Gets the nearest player to a component
    public static (PlayerTank Tank, TankData Data) GetNearestPlayer<T>(T component) where T : Component
    {
        return GetNearestPlayer(component.gameObject);
    }

    //Called when all the enemy tanks in the map have been destroyed
    public static void Win(PlayerTank Winner,bool PlayWinSound = true)
    {
        //Set the winning player's UI to the win screen and allow that player to access the menu buttons
        Winner.UI.ButtonsEnabled = true;
        Winner.UI.SetUIState("Win", Curves.Smoothest, FromIsHidden: true);
        //Play the win sound if set to true
        if (PlayWinSound)
        {
            Audio.Play(Game.WinSound, Audio.SoundEffects, Vector3.zero, Vector3.zero);
        }
        //Stop playing the level
        PlayingLevel = false;
        //Show the winner's final score on the results screen
        Winner.UI.FinalScore = Winner.Score;
    }

    //Called when the player tank has been destroyed
    public static void Lose(PlayerTank Loser,bool PlayLoseSound = true)
    {
        //If this player is the last player to die
        if (Players.Count == 0)
        {
            //Enable the buttons for this player
            Loser.UI.ButtonsEnabled = true;
            //Stop playing the level
            PlayingLevel = false;
        }
        else
        {
            //Hide the buttons to prevent the player from closing down the level, while another player is still playing
            Loser.UI.ButtonsEnabled = false;
        }
        //Play the lose sound if enabled
        if (PlayLoseSound)
        {
            Audio.Play(Game.LoseSound, Audio.SoundEffects,Vector3.zero,Vector3.zero);
        }
        //Show the losing screen
        Loser.UI.SetUIState("Lose", Curves.Smoothest, FromIsHidden: true);
        //Show the player's final score
        Loser.UI.FinalScore = Loser.Score;
    }

    //Adds a function that is called when the level unloads
    [RuntimeInitializeOnLoadMethod]
    private static void UnloadHandler()
    {
        OnLevelUnload += () =>
        {
            Paused = false;
            PausedUI = null;
            PlayingLevel = false;
            Players.Clear();
            Enemies.Clear();
        };
    }

    //A routine to unload the level
    public static IEnumerator UnloadLevel(bool GoingToMainMenu)
    {
        //If the game is loaded
        if (SceneManager.GetSceneByName("Game").isLoaded)
        {
            //Call the OnLevelUnload Event
            OnLevelUnload?.Invoke();
            //Unload it
            yield return SceneManager.UnloadSceneAsync("Game");
            //If the game is going to the main menu
            if (GoingToMainMenu)
            {
                //Play the menu music
                PlayMusic(MusicType.Menu);
                //Start the background panorama
                yield return PanoramaGenerator.StartPanorama();
            }
        }
    }

    //Plays a specific type of music
    private static void PlayMusic(MusicType type,float volumeModifier = 1f)
    {
        //Stop the existing music from playing
        if (Game.MusicObject != null)
        {
            Game.MusicObject.Stop();
            Game.MusicObject = null;
        }
        switch (type)
        {
            case MusicType.None: //Play nothing
                break;
            case MusicType.Menu: //Play the menu music
                Game.MusicObject = Audio.Play(Game.MenuMusic, () => Audio.MusicVolume * volumeModifier, true, AudioPlayType.Stereo);
                break;
            case MusicType.Game: //Play the game music
                Game.MusicObject = Audio.Play(Game.GameMusic.RandomElement(), () => Audio.MusicVolume * volumeModifier, true, AudioPlayType.Stereo);
                break;
        }
    }

    //A routine to load the level and play the game
    static IEnumerator LoadGameScene(LevelLoadMode loadMode)
    {
        //If there is a level already loaded, unload it
        yield return UnloadLevel(false);
        //Stop the panorama
        yield return PanoramaGenerator.StopPanorama();
        //Stop playing the music
        PlayMusic(MusicType.None);
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
        CurrentGameColor = Color.HSVToRGB(Random.value, 1f, 1f);

        //Get a copy of the possible player spawnpoints
        var playerSpawns = MapGenerator.PlayerSpawnPoints.Clone();

        //Spawn the first player at a random spawnpoint
        PlayerTank.Create(playerSpawns.PopRandom().transform.position, 1,false);
        //If there are two players playing
        if (Options.PlayerCount.Value == PlayerCount.TwoPlayers)
        {
            //Spawn the second player tank at a random spawnpoint
            PlayerTank.Create(playerSpawns.PopRandom().transform.position, 2);
        }
        //Enable the border for all of the player screens
        foreach (var screen in MultiplayerScreens.GetAllScreens())
        {
            screen.PlayerUI.Border = true;
        }
        //Show the ready sequence
        yield return UI.ShowReadySequence();
        //Show the game UI for all screens and play the game music
        PlayMusic(MusicType.Game,0.5f);
        UIManager.All.SetUIState("Game");
        //Set the playing level flag
        PlayingLevel = true;
    }

    //Gets the highscore for a specific player
    public static float GetHighScoreFor(int PlayerNumber)
    {
        return PlayerPrefs.GetFloat("HighscorePlayer" + PlayerNumber);
    }

    //Sets the highscore for a specific player
    public static void SetHighScoreFor(int PlayerNumber, float score)
    {
        PlayerPrefs.SetFloat("HighscorePlayer" + PlayerNumber, score);
    }

}
