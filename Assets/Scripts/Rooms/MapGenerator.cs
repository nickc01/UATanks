using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    [Header("Map Generator Settigns")]
    [Tooltip("How many tiles wide the map will be")]
    public int MapWidth = 5;
    [Tooltip("How many tiles long the map will be")]
    public int MapHeight = 5;
    [Tooltip("How wide and how long each tile in the map will be")]
    public Vector2Int TileDimensions;
    [Tooltip("The seed used to generate the map")]
    public int Seed = 0;
    [Tooltip("The type of seed to use")]
    public SeedGenerator SeedGenerator;
    [Tooltip("A list of room prefabs to use")]
    public List<GameObject> Rooms = new List<GameObject>();
    [Header("Debug")]
    [Tooltip("Generates the map upon playing the game")]
    public bool Test = false;

    public static List<PlayerSpawn> PlayerSpawnPoints = new List<PlayerSpawn>();

    public void GenerateMap()
    {
        PlayerSpawnPoints.Clear();
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

            }
        }
    }

    private int GetRandomSeed()
    {
        return DateTime.Now.GetHashCode();
    }
    private int GetSeedOfTheDay()
    {
        return DateTime.Today.GetHashCode();
    }


    private void Start()
    {
        if (Test)
        {
            GenerateMap();
        }
    }

}
