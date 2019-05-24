using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static GameManager;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generator Settigns")]
    [Tooltip("How many tiles wide the map will be")]
    public int MapWidth = 1;
    [Tooltip("How many tiles long the map will be")]
    public int MapHeight = 2;
    [Tooltip("Increases the width by 1 after a set amount of levels")]
    public int IncreaseWidthEvery = 2;
    [Tooltip("Increases the height by 1 after a set amount of levels")]
    public int IncreaseHeightEvery = 2;
    [Tooltip("How wide and how long each tile in the map will be")]
    public Vector2Int TileDimensions;
    [Tooltip("The type of seed to use")]
    [PropSender("SeedGenerator", "SeedGenerator", SeedGenerator.UseSeed)]
    public SeedGenerator SeedGenerator;
    [Tooltip("The seed used to generate the map")]
    [PropReceiver("SeedGenerator")]
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
                Random.InitState(GetRandomSeed());
                break;
            case SeedGenerator.MapOfTheDay:
                Random.InitState(GetSeedOfTheDay());
                break;
        }
    }

    //Generates the map
    public void GenerateMap(int level = 0)
    {
        //Reset the seed
        ResetSeed();
        //Clear the list of player spawnpoints
        PlayerSpawnPoints.Clear();

        //Set the width and height of the map generator
        var width = Mathf.FloorToInt(level / (float)IncreaseWidthEvery) + MapWidth;
        var height = Mathf.FloorToInt(level / (float)IncreaseHeightEvery) + MapHeight;

        //Loop over each tiles in the grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Instantiate a random room
                var NewRoom = Instantiate(Rooms[Random.Range(0, Rooms.Count)], new Vector3(x * TileDimensions.x,0, y * TileDimensions.y), Quaternion.identity);
                //Open up the doors in the room
                foreach (var door in NewRoom.GetComponentsInChildren<Door>())
                {
                    switch (door.direction)
                    {
                        case DoorDirection.Up when y < height - 1:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Down when y > 0:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Left when x > 0:
                            door.gameObject.SetActive(false);
                            break;
                        case DoorDirection.Right when x < width - 1:
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
                    //Instantiate a new random enemy
                    var enemy = Instantiate(Game.EnemyPrefabs[Random.Range(0, Game.EnemyPrefabs.Count)], enemySpawn.transform.position, enemySpawn.transform.rotation).GetComponent<EnemyTank>();
                    //If the enemy has the patrol personality
                    if (enemy.Personality == Personality.Patrol)
                    {
                        //Get and Set the enemy's patrol points
                        var AllPatrolSets = NewRoom.GetComponentsInChildren<PatrolSet>();
                        var SelectedPatrolSet = AllPatrolSets[Random.Range(0, AllPatrolSets.GetLength(0))];
;                       enemy.PatrolPoints.AddRange(SelectedPatrolSet.GetComponentsInChildren<Transform>());
                    }
                }
            }
        }
    }

    //Returns a spawnpoint for the player to spawn at
    //It also removes the spawnpoint from the list to prevent spawning at duplicate places
    public PlayerSpawn PopPlayerSpawnPoint()
    {
        //Initialize the seed
        ResetSeed();
        var spawnPoint = PlayerSpawnPoints[Random.Range(0,PlayerSpawnPoints.Count)];
        PlayerSpawnPoints.Remove(spawnPoint);
        return spawnPoint;
    }

    private int GetRandomSeed()
    {
        //Get the time right now and convert it to a hash code for use as a seed
        return DateTime.Now.GetHashCode();
    }
    private int GetSeedOfTheDay()
    {
        //Get the time for just today and convert it to a hash code for use as a seed
        return DateTime.Today.GetHashCode();
    }
}
