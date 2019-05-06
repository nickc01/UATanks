using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game { get; private set; } //The singleton for the game manager
    public static TankData Player; //The data of the current player in the game
    public static List<TankData> Enemies = new List<TankData>(); //The data of all the enemies in the game

    [Header("Prefabs")]
    [Tooltip("The prefab used whenever a tank fires a shell")]
    public GameObject ShellPrefab;


    

    private void Start()
    {
        //Set the singleton
        if (Game == null)
        {
            Game = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
