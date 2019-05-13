using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Game { get; private set; } //The singleton for the game manager
    public static (PlayerTank Tank,TankData Data) Player; //The data of the current player in the game
    public static List<(EnemyTank Tank,TankData Data)> Enemies = new List<(EnemyTank,TankData)>(); //The data of all the enemies in the game

    [Header("Prefabs")]
    [Tooltip("The prefab used whenever a tank fires a shell")]
    public GameObject ShellPrefab;

    [Header("Sounds")]
    [Tooltip("Played When a shell is fired")]
    public AudioClip FireSound;
    [Tooltip("Played when you win the game")]
    public AudioClip WinSound;
    [Tooltip("Played when you lose the game")]
    public AudioClip LoseSound;

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
    }

    //Called when all the enemy tanks in the map have been destroyed
    public static void Win()
    {
        //Show the win screen
        UIManager.SetUIState("Win");
        //Play the Win Sound
        CameraController.Main.Sound.clip = Game.WinSound;
        CameraController.Main.Sound.Play();
    }

    //Called when the player tank has been destroyed
    public static void Lose()
    {
        //Show the lose screen
        UIManager.SetUIState("Lose");
        //Play the Lose Sound
        CameraController.Main.Sound.clip = Game.LoseSound;
        CameraController.Main.Sound.Play();
    }

    //Called when the play button is pressed
    //Used to start the game
    public static void Play()
    {
        //Show the game UI
        UIManager.SetUIState("Game");
        //Load the game scene
        Game.StartCoroutine(LoadGameScene());
    }

    static IEnumerator LoadGameScene()
    {
        if (!SceneManager.GetSceneByName("Game").isLoaded)
        {
            //Load the game scene
            yield return SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        }
        //Set the scene active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
    }

}
