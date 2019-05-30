using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : PlayerSpecific
{
    public Camera CameraComponent { get; private set; }
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


    private void Start()
    {
        //MultiplayerManager.AddedPlayersUpdate += PlayerCountChanged;
        CameraComponent = GetComponent<Camera>();
        OnNewPlayerChange();
    }

    /*private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MultiplayerManager.AddedPlayersUpdate -= PlayerCountChanged;
        }
    }*/

    private int AddToMask(int originalMask,int layerNumber)
    {
        return originalMask | (1 << layerNumber);
    }

    private int RemoveFromMask(int originalMask, int layerNumber)
    {
        return originalMask & ~(1 << layerNumber);
    }

    public override void OnNewPlayerChange()
    {
        if (PlayerNumber > 1)
        {
            var originalMask = MultiplayerManager.Primary.PlayerCamera.CameraComponent.cullingMask;
            originalMask = RemoveFromMask(originalMask, LayerMask.NameToLayer("UIPlayer1"));
            originalMask = AddToMask(originalMask, LayerMask.NameToLayer("UIPlayer" + PlayerNumber));
            MultiplayerManager.GetPlayerInfo(PlayerNumber).PlayerCamera.CameraComponent.cullingMask = originalMask;
        }
        if (MultiplayerManager.PlayersAdded == 1)
        {
            CameraComponent.depth = -1;
            CameraComponent.rect = new Rect(0, 0, 1, 1);
        }
        else if (MultiplayerManager.PlayersAdded == 2)
        {
            if (PlayerNumber == 1)
            {
                CameraComponent.depth = -1;
                CameraComponent.rect = new Rect(0, 0, 0.5f, 1);
            }
            else
            {
                CameraComponent.depth = 0;
                CameraComponent.rect = new Rect(0.5f, 0, 1, 1);
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
    //public static (float Width, float Height) GetCameraBounds() => (Camera.main.pixelWidth, Camera.main.pixelHeight);

}
