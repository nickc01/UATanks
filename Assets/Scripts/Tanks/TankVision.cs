using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TankVision : MonoBehaviour
{
    public float SightRange { get; set; } = 5f;
    public float SightFOV { get; set; } = 45f;
    [Tooltip("A list of layers that will respond to the raycasts sent by the component")]
    [SerializeField] LayerMask Blockers;

    public bool SeeingTarget(Vector3 target)
    {
        return Physics.Raycast(transform.position, (target - transform.position).normalized, out var hitInfo, SightRange, Blockers) && hitInfo.transform.position == target;
    }
}
