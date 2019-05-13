using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TankHearing : MonoBehaviour
{
    public float HearingRange { get; set; } = 5f; //How far this tank can hear

    //Returns true if this tank can hear the target
    //Optionally, you can also put in the amount of noise the target is emitting
    public bool HearingTarget(Vector3 target, float targetNoise = 0f)
    {
        return Vector3.Distance(transform.position, target) - targetNoise <= HearingRange;
    }
    public bool HearingTarget(Vector3 target, NoiseMaker noise)
    {
        return HearingTarget(target, noise.NoiseLevel);
    }
}
