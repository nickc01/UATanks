using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class Vision
{
    public Transform Source { get; set; } //The source object
    public float SightRange { get; set; } //How far the source can see
    public float SightFOV { get; set; } //The field of view of the source
    public LayerMask Blockers; //The objects that will block the line of sight

    RaycastHit[] hit = new RaycastHit[1];

    //Returns true if the source can see the target
    public bool CanSeeTarget(Vector3 target)
    {
        if (Source == null)
        {
            return false;
        }
        //Fire a raycast towards the target and check to see if the the ray has collided with the target
        return Physics.RaycastNonAlloc(Source.transform.position, (target - Source.transform.position).normalized, hit, SightRange, Blockers) > 0 && hit[0].transform.position == target;
        //return Physics.Raycast(Source.transform.position, (target - Source.transform.position).normalized, out var hitInfo, SightRange, Blockers) && hitInfo.transform.position == target;
    }

    //Used to create the vision object
    public Vision(Transform source = null, float sightRange = 5f, float sightFOV = 45f, LayerMask blockers = default)
    {
        Source = source;
        SightRange = sightRange;
        SightFOV = sightFOV;
        Blockers = blockers;
    }
}
