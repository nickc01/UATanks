using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct MinimapObject
{
    public Transform Target;
    public GameObject RenderPrefab;

    public MinimapObject(Transform target, GameObject renderPrefab)
    {
        Target = target;
        RenderPrefab = renderPrefab;
    }
}

public static class MinimapManager
{
    public static List<Camera> MinimapCameras = new List<Camera>();
    //public static List<MinimapObject> RenderObjects = new List<MinimapObject>();
    public static Dictionary<Transform, GameObject> RenderObjects = new Dictionary<Transform, GameObject>();

    //public static ReadOnlyCollection<MinimapObject> RenderObjects => renderObjects.AsReadOnly();

    public static void AddTarget(Transform target, GameObject prefab)
    {
        RenderObjects.Add(target,prefab);
        OnTargetAdd?.Invoke(target,prefab);
    }

    public static void RemoveTarget(Transform target)
    {
        OnTargetRemove?.Invoke(target, RenderObjects[target]);
        RenderObjects.Remove(target);
    }

    public static event Action<Transform,GameObject> OnTargetAdd;
    public static event Action<Transform, GameObject> OnTargetRemove;

}
