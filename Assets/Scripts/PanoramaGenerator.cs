using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PanoramaGenerator : MonoBehaviour
{
    [SerializeField] Transform RoomHolder; //The object that holds all the rooms
    [SerializeField] float MovementSpeed = 4f; //How fast the rooms move
    [SerializeField] float MovementDirection = 22.5f; //The direction the rooms move in
    [SerializeField] Vector2Int RoomArraySize = new Vector2Int(6, 6); //How many rooms will there be

    Vector2 movementTracker = new Vector2(0f, 0f);
    Vector2 movementTotalTracker = new Vector2(0f, 0f);
    Vector3 Direction;
    Vector2 RoomOffset;
    Rect Boundaries;
    MapGenerator Map => MapGenerator.Generator; //An easier way to getting the map generator
    List<GameObject> Rooms = new List<GameObject>(); //A list of generated rooms

    //Loads the panorama scene
    public static IEnumerator StartPanorama()
    {
        var panorama = SceneManager.GetSceneByName("Panorama");
        if (!panorama.isLoaded)
        {
            yield return SceneManager.LoadSceneAsync("Panorama",LoadSceneMode.Additive);
        }
    }

    //Stops the panorama scene
    public static IEnumerator StopPanorama()
    {
        var panorama = SceneManager.GetSceneByName("Panorama");
        if (panorama.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(panorama);
        }
    }

    private void Start()
    {
        //Set the panorama scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Panorama"));
        //Initialize the room offset for centering of the rooms
        RoomOffset = ((Vector2)RoomArraySize) / 2f * Map.TileDimensions;
        //Initialize the direction vector
        Direction = new Vector3(Mathf.Cos(Mathf.Deg2Rad * MovementDirection),0f, Mathf.Sin(Mathf.Deg2Rad * MovementDirection));
        //Spawn each of the rooms and add them to the array
        for (int x = 0; x < RoomArraySize.x; x++)
        {
            for (int y = 0; y < RoomArraySize.y; y++)
            {
                var newRoom = GameObject.Instantiate(Map.Rooms.RandomElement());
                //Disable the doors
                foreach (var door in newRoom.GetComponentsInChildren<Door>())
                {
                    door.gameObject.SetActive(false);
                }
                newRoom.transform.SetParent(RoomHolder);
                newRoom.transform.localPosition = new Vector3(-RoomOffset.x + (x * Map.TileDimensions.x), 0f, -RoomOffset.y + (y * Map.TileDimensions.y));
                Rooms.Add(newRoom);
            }
        }
        //Set the boundaries
        Boundaries = new Rect(-RoomOffset, RoomArraySize * Map.TileDimensions);
        //Set the main scene as active
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    private void Update()
    {
        //If the rooms are exiting the boundaries, teleport them to the other side
        foreach (var room in Rooms)
        {
            room.transform.localPosition += Direction * Time.deltaTime * MovementSpeed;
            if (room.transform.localPosition.x >= Boundaries.xMax)
            {
                room.transform.localPosition = new Vector3(room.transform.localPosition.x - Boundaries.width,0, room.transform.localPosition.z);
            }
            if (room.transform.localPosition.z >= Boundaries.yMax)
            {
                room.transform.localPosition = new Vector3(room.transform.localPosition.x,0, room.transform.localPosition.z - Boundaries.height);
            }
            if (room.transform.localPosition.x < Boundaries.xMin)
            {
                room.transform.localPosition = new Vector3(room.transform.localPosition.x + Boundaries.width, 0, room.transform.localPosition.z);
            }
            if (room.transform.localPosition.z < Boundaries.yMin)
            {
                room.transform.localPosition = new Vector3(room.transform.localPosition.x,0, room.transform.localPosition.z + Boundaries.height);
            }
        }
    }
}