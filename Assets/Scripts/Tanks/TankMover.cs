using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Extensions;

[RequireComponent(typeof(CharacterController))]
public class TankMover : MonoBehaviour
{
    CharacterController controller;
    public ObstacleAvoidance OA { get; private set; }
    private void Start()
    {
        //Gets the character controller of this tank
        controller = GetComponent<CharacterController>();
        //Create the Obstacle Avoidance System for this mover
        OA = new ObstacleAvoidance(transform);
    }
    //Moves the tank forward at a set speed
    //Negative values move the tank backwards
    public void Move(float speed)
    {
        controller.SimpleMove(transform.forward * speed);
    }

    //Rotates the tank by a set amount of degrees
    //Negative values rotate the tank to the left
    //If Use OA is set to true, then it will make sure to avoid any obstacles as well
    public void Rotate(float degrees, bool UseOA = false)
    {
        if (UseOA)
        {
            if (OA == null)
            {
                throw new Exception("There is no Obstacle Avoidance Component attached to this object, please add one to use Obstacle Avoidance");
            }
            degrees += degrees * OA.RecommendedDirection;
        }
        transform.Rotate(0, degrees, 0);
    }

    //Rotates towards a specified target with a specific amount of degrees
    //If Use OA is set to true, then it will make sure to avoid any obstacles as well
    public void RotateTowards(Vector3 target, float maxDegrees,bool UseOA = false)
    {

        var angle = 0f;
        //angle = ClampAbs(angle, 0, Mathf.Abs(maxDegrees));
        if (maxDegrees < 0f)
        {
            //angle = -Mathf.Clamp(Mathf.Abs(angle),0,maxDegrees);
            angle = GetAngleAwayFrom(target);
            angle = ClampAbs(angle, 0, Mathf.Abs(maxDegrees));
        }
        else
        {
            angle = GetAngleTo(target);
            angle = ClampAbs(angle, 0, Mathf.Abs(maxDegrees));
        }
        if (UseOA)
        {
            if (OA == null)
            {
                throw new Exception("There is no Obstacle Avoidance Component attached to this object, please add one to use Obstacle Avoidance");
            }
            //Rotate(OA.RecommendedDirection * Mathf.Abs(maxDegrees), false);
            Rotate(BiggerAbsNumber(OA.RecommendedDirection * Mathf.Abs(maxDegrees),angle), false);
        }
        else
        {
            Rotate(angle);
        }
    }

    public float GetAngleTo(Vector3 target)
    {
        return Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);
    }

    public float GetAngleAwayFrom(Vector3 target)
    {
        return Vector3.SignedAngle(transform.forward, -(target - transform.position), Vector3.up);
    }
}
