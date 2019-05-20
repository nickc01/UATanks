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
    public int MapWidth = 5;
    [Tooltip("How many tiles long the map will be")]
    public int MapHeight = 5;
    [Tooltip("How wide and how long each tile in the map will be")]
    public Vector2Int TileDimensions;
    [Tooltip("The type of seed to use")]
    [HiddenSender("SeedGenerator", "SeedGenerator", SeedGenerator.UseSeed)]
    public SeedGenerator SeedGenerator;
    [Tooltip("The seed used to generate the map")]
    [HiddenReceiver("SeedGenerator")]
    public int Seed = 0;
    [Tooltip("A list of room prefabs to use")]
    public List<GameObject> Rooms = new List<GameObject>();

    public static List<PlayerSpawn> PlayerSpawnPoints = new List<PlayerSpawn>();
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

    public void GenerateMap()
    {
        ResetSeed();
        PlayerSpawnPoints.Clear();
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                var NewRoom = Instantiate(Rooms[Random.Range(0, Rooms.Count)], new Vector3(x * TileDimensions.x,0, y * TileDimensions.y), Quaternion.identity);
                //Open Up the Doors
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
                    var enemy = Instantiate(Game.EnemyPrefabs[Random.Range(0, Game.EnemyPrefabs.Count)], enemySpawn.transform.position, enemySpawn.transform.rotation).GetComponent<EnemyTank>();
                    if (enemy.Personality == Personality.Patrol)
                    {
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
        return DateTime.Now.GetHashCode();
    }
    private int GetSeedOfTheDay()
    {
        return DateTime.Today.GetHashCode();
    }
}
