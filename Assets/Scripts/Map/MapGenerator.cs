using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameManager;

public class MapGenerator : MonoBehaviour
{
    public int MapWidth => Options.MapWidth; //How wide the map will be
    public int MapHeight => Options.MapHeight; //How long the map will be

    [Header("Map Generator Settings")]
    [Tooltip("How wide and how long each tile in the map will be")]
    public Vector2Int TileDimensions;
    [Tooltip("The type of seed to use")]
    [HideInInspector]
    public SeedGenerator SeedGenerator;
    [Tooltip("The seed used to generate the map")]
    [HideInInspector]
    public int Seed = 0;
    [Tooltip("A list of room prefabs to use")]
    public List<GameObject> Rooms = new List<GameObject>();

    public static List<PlayerSpawn> PlayerSpawnPoints = new List<PlayerSpawn>(); //The list of player spawnpoints
    public static MapGenerator Generator { get; private set; } //The singleton for the map generator

    private void Start()
    {
        //Set the singleton
        if (Generator == null)
        {
            Generator = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void ResetSeed()
    {
        //Reset the seed based on the type of seed generator used
        switch (SeedGenerator)
        {
            case SeedGenerator.UseSeed:
                Random.InitState(Seed);
                break;
            case SeedGenerator.Random:
                Random.InitState(Seed = GetRandomSeed());
                break;
            case SeedGenerator.MapOfTheDay:
                Random.InitState(Seed = GetSeedOfTheDay());
                break;
        }
    }

    //Generates the map
    public void GenerateMap(int seed = 0)
    {
        Seed = seed;
        //Reset the seed
        ResetSeed();
        //Clear the list of player spawnpoints
        PlayerSpawnPoints.Clear();

        //Loop over each tiles in the grid
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                //Instantiate a random room
                var NewRoom = Instantiate(Rooms[Random.Range(0, Rooms.Count)], new Vector3(x * TileDimensions.x,0, y * TileDimensions.y), Quaternion.identity);
                //Open up the doors in the room
                foreach (var door in NewRoom.GetComponentsInChildren<Door>())
                {
                    switch (door.direction)
                    {
                        case DoorDirection.Up when y < MapHeight - 1:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Down when y > 0:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Left when x > 0:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Right when x < MapWidth - 1:
                            door.gameObject.SetActive(false);
                            break;
                        default:
                            door.gameObject.SetActive(true);
                            break;
                    }
                }
                //Add the player Spawnpoints
                PlayerSpawnPoints.AddRange(NewRoom.GetComponentsInChildren<PlayerSpawn>());
                //Spawn Random Enemies
                foreach (var enemySpawn in NewRoom.GetComponentsInChildren<EnemySpawn>())
                {
                    EnemyTank SpawnEnemy(bool random = true)
                    {
                        Vector3 spawnpoint = Vector3.zero;
                        if (random)
                        {
                            spawnpoint = enemySpawn.transform.position + new Vector3(Random.value, 0, Random.value).normalized * 2f;
                        }
                        else
                        {
                            spawnpoint = enemySpawn.transform.position;
                        }
                        //Instantiate a new random enemy
                        var enemy = Instantiate(Game.EnemyPrefabs[Random.Range(0, Game.EnemyPrefabs.Count)], spawnpoint, enemySpawn.transform.rotation).GetComponent<EnemyTank>();
                        //If the enemy has the patrol personality
                        if (enemy.Personality == Personality.Patrol)
                        {
                            //Get and Set the enemy's patrol points
                            var AllPatrolSets = NewRoom.GetComponentsInChildren<PatrolSet>();
                            var SelectedPatrolSet = AllPatrolSets[Random.Range(0, AllPatrolSets.GetLength(0))];
                            enemy.PatrolPoints.AddRange(SelectedPatrolSet.GetComponentsInChildren<Transform>());
                        }
                        return enemy;
                    }
                    var newEnemy = SpawnEnemy(false);
                    if (Options.Difficulty.Value == Difficulty.Easy)
                    {
                        break;
                    }
                    if (Options.Difficulty.Value == Difficulty.Hard)
                    {
                        //Spawn another enemy!
                        //SpawnEnemy();
                        var data = newEnemy.GetComponent<TankData>();
                        data.MaxLives += 1;
                        data.Lives += 1;
                        //newEnemy.Data.MaxLives += 1;
                        //newEnemy.Data.Lives += 1;
                    }
                }
            }
        }
    }

    //Gets a random seed
    private int GetRandomSeed()
    {
        //Get the time right now and convert it to a hash code for use as a seed
        return DateTime.Now.GetHashCode();
    }

    //Gets the map of the day
    private int GetSeedOfTheDay()
    {
        //Get the time for just today and convert it to a hash code for use as a seed
        return DateTime.Today.GetHashCode();
    }
}
