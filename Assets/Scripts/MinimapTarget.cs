using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapTarget : MonoBehaviour
{
    private RawImage targetInternal;
    public RawImage Target //The render texture target for the minimap camera
    {
        get
        {
            if (targetInternal == null)
            {
                targetInternal = GetComponent<RawImage>();
            }
            return targetInternal;
        }
    }

    [HideInInspector]
    public Camera MinimapCamera; //The minimap camera

    RectTransform RTransform; //The rect transform component

    private float GetCamHeight() => MinimapCamera.orthographicSize * 2f; //Gets the camera's height
    private float GetCamWidth() => MinimapCamera.aspect * GetCamHeight(); //Gets the camera's width
    private Vector3 CamPosition => MinimapCamera.transform.position; //Gets the camera's position

    private List<Transform> RenderObjects = new List<Transform>(); //The list of icons on the minimap to render
    private Dictionary<Transform, RectTransform> RenderInstances = new Dictionary<Transform, RectTransform>(); //Stores the rectTransforms of the Icons

    //Called when a new icon is added
    private void TargetAdded(Transform target, GameObject prefab)
    {
        RenderObjects.Add(target);
        var Copy = Instantiate(prefab, transform).GetComponent<MinimapPrefab>();
        Copy.transform.localPosition = Vector3.zero;
        Copy.transform.localRotation = Quaternion.identity;
        Copy.Source = target;
        RenderInstances.Add(target, Copy.GetComponent<RectTransform>());
    }

    //Called when an icon is removed
    private void TargetRemoved(Transform target, GameObject prefab)
    {
        RenderObjects.Remove(target);
        var instance = RenderInstances[target];
        Destroy(instance.gameObject);
        RenderInstances.Remove(target);
    }


    private void Start()
    {
        RTransform = GetComponent<RectTransform>();
        MinimapManager.OnTargetAdd += TargetAdded;
        MinimapManager.OnTargetRemove += TargetRemoved;
        foreach (var obj in MinimapManager.RenderObjects)
        {
            TargetAdded(obj.Key,obj.Value);
        }
    }

    private void Update()
    {
        if (MinimapCamera != null)
        {
            //Updates all the icons's positions
            var Height = GetCamHeight();
            var Width = GetCamWidth();
            Rect camRect = new Rect(new Vector2(CamPosition.x, CamPosition.z) - (new Vector2(Width, Height) / 2f),new Vector2(Width,Height));
            foreach (var target in RenderObjects)
            {
                Vector2 relativeCoords = new Vector2(Mathf.InverseLerp(camRect.xMin, camRect.xMax, target.position.x), Mathf.InverseLerp(camRect.yMin, camRect.yMax, target.position.z));
                var instance = RenderInstances[target];
                var minimapRect = RTransform.rect;
                instance.anchoredPosition = new Vector2(Mathf.Lerp(minimapRect.xMin, minimapRect.xMax, relativeCoords.x), Mathf.Lerp(minimapRect.yMin, minimapRect.yMax, relativeCoords.y));
            }
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            MinimapManager.OnTargetAdd -= TargetAdded;
            MinimapManager.OnTargetRemove -= TargetRemoved;
        }
    }

}
