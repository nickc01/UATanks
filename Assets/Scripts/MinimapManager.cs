using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MinimapManager
{
    public static List<Camera> MinimapCameras = new List<Camera>(); //A list of all the minimap cameras in the game
    public static Dictionary<Transform, GameObject> RenderObjects = new Dictionary<Transform, GameObject>(); //A list of icons to render

    //Called when the level unloads
    [RuntimeInitializeOnLoadMethod]
    private static void UnloadHandler()
    {
        GameManager.OnLevelUnload += () => {
            RemoveAllTargets();
        };
    }

    //Used to add an icon
    public static void AddTarget(Transform target, GameObject prefab)
    {
        RenderObjects.Add(target,prefab);
        OnTargetAdd?.Invoke(target,prefab);
    }


    //Used to remove an icon
    public static void RemoveTarget(Transform target)
    {
        if (RenderObjects.ContainsKey(target))
        {
            OnTargetRemove?.Invoke(target, RenderObjects[target]);
            RenderObjects.Remove(target);
        }
    }

    //Deletes all the icons
    public static void RemoveAllTargets()
    {
        foreach (var target in RenderObjects)
        {
            OnTargetRemove?.Invoke(target.Key, RenderObjects[target.Key]);
        }
        RenderObjects.Clear();
    }

    public static event Action<Transform,GameObject> OnTargetAdd; //Called when a target is added
    public static event Action<Transform, GameObject> OnTargetRemove; //Called when a target is removed

}
