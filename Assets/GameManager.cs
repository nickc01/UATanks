using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Game { get; private set; } //The singleton for the game manager

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
