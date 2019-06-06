using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : PlayerSpecific
{
    public Camera CameraComponent { get; private set; }
    public float Speed = 7f; //How fast the camera moves towards the target

    private GameObject targetInternal;
    public GameObject Target
    {
        get => targetInternal;
        set => SetTarget(value);
    }

    Camera MinimapCamera;
    RenderTexture MinimapRenderTexture;
    [SerializeField] Vector2Int RenderTextureDimensions = new Vector2Int(512, 512);


    private void Start()
    {
        MinimapRenderTexture = new RenderTexture(RenderTextureDimensions.x, RenderTextureDimensions.y, 0);
        //MultiplayerManager.AddedPlayersUpdate += PlayerCountChanged;
        CameraComponent = GetComponent<Camera>();
        //MinimapCamera = GetComponentOnlyInChildren<Camera>(true);
        //MinimapCamera = this.GetComponentOnlyInChildren<Camera>(true);
        MinimapCamera = transform.Find("Minimap Camera").GetComponent<Camera>();
        MinimapCamera.targetTexture = MinimapRenderTexture;
        MinimapManager.MinimapCameras.Add(MinimapCamera);
        OnNewPlayerChange();
        Debug.Log("TEST");
    }

    //Adds a layer to the mask
    private void AddToMask(ref int originalMask,int layerNumber)
    {
        originalMask |= (1 << layerNumber);
    }

    //Removes a layer from the mask
    private void RemoveFromMask(ref int originalMask, int layerNumber)
    {
        originalMask &= ~(1 << layerNumber);
    }

    public override void OnNewPlayerChange()
    {
        if (PlayerNumber > 1)
        {
            //Get the original mask
            var mask = MultiplayerScreens.Primary.PlayerCamera.CameraComponent.cullingMask;
            //Remove the "UIPlayer1" Layer
            RemoveFromMask(ref mask, LayerMask.NameToLayer("UIPlayer1"));
            //Add the layer that corresponds to the current player number
            AddToMask(ref mask, LayerMask.NameToLayer("UIPlayer" + PlayerNumber));
            //Set the cameras culling mask to the new mask
            MultiplayerScreens.GetPlayerScreen(PlayerNumber).PlayerCamera.CameraComponent.cullingMask = mask;
        }
        var MinimapTarget = MultiplayerScreens.GetPlayerScreen(PlayerNumber).PlayerUI.GetComponentInChildren<MinimapTarget>(true);
        MinimapTarget.Target.texture = MinimapRenderTexture;
        MinimapTarget.MinimapCamera = MinimapCamera;
        //Set the camera's viewport
        if (MultiplayerScreens.PlayersAdded == 1)
        {
            CameraComponent.depth = -1;
            CameraComponent.rect = new Rect(0, 0, 1, 1);
        }
        else if (MultiplayerScreens.PlayersAdded == 2)
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

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MinimapManager.MinimapCameras.Remove(MinimapCamera);
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
        if (target != null)
        {
            //Set the position of the camera to the target
            transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        }
        else
        {
            transform.position = new Vector3(0f, 15f, 0f);
        }
    }

}
