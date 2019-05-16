using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Hearing
{
    public Transform Source { get; set; } //The source object
    public float HearingRange { get; set; } //The hearing range of the source
    //Returns true if the source object can hear the target
    //Optionally, you can also put in the amount of noise the target is emitting
    public bool CanHearTarget(Vector3 Target, float targetNoise = 0f)
    {
        if (Source == null)
        {
            return false;
        }
        return Vector3.Distance(Source.transform.position, Target) - targetNoise <= HearingRange;
    }

    //Used to create the hearing object
    public Hearing(Transform source = null, float hearingRange = 5f)
    {
        Source = source;
        HearingRange = hearingRange;
    }
}
