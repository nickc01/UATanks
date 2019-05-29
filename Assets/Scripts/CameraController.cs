using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IIsPlayerSpecific
{
    Camera currentCamera;
    public float Speed = 7f; //How fast the camera moves towards the target

    //private static GameObject targetInternal; //The target the camera will move towards
    //public static CameraController Main { get; private set; } //The singleton for the camera controller

    /*public static GameObject Target
    {
        get => targetInternal;
        set => SetTarget(value);
    }*/
    //int IIsPlayerSpecific.PlayerID { get; set; }

    private GameObject targetInternal;
    public GameObject Target
    {
        get => targetInternal;
        set => SetTarget(value);
    }

    public int PlayerID { get; set; }

    //public AudioSource Sound { get; private set; } //The Audio Source of the Camera

    private void Start()
    {
        //Set the singleton
        /*if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/
        MultiplayerManager.AddedPlayersUpdate += PlayerCountChanged;
        currentCamera = GetComponent<Camera>();
        //Set the Audio Source
        //Sound = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MultiplayerManager.AddedPlayersUpdate -= PlayerCountChanged;
        }
    }

    private void PlayerCountChanged()
    {
        if (MultiplayerManager.PlayersAdded == 1)
        {
            currentCamera.depth = -1;
            currentCamera.rect = new Rect(0, 0, 1, 1);
        }
        else if (MultiplayerManager.PlayersAdded == 2)
        {
            if (PlayerID == 1)
            {
                currentCamera.depth = -1;
                currentCamera.rect = new Rect(0, 0, 0.5f, 0.5f);
            }
            else
            {
                currentCamera.depth = 0;
                currentCamera.rect = new Rect(0.5f, 0.5f, 1, 1);
            }
        }
    }

    private void Update()
    {
        //If there is a target to move to
        if (targetInternal != null)
        {
            //Linearly interpolate to the target
            Vector3 targetPosition = targetInternal.transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), Time.deltaTime * Speed);
        }
    }

    //Sets the target that the camera should move to
    private void SetTarget(GameObject target)
    {
        targetInternal = target;
        //Set the position of the camera to the target
        transform.position = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
    }

    //Get the width and height of the main camera
    public static (float Width, float Height) GetCameraBounds() => (Camera.main.pixelWidth, Camera.main.pixelHeight);

}
