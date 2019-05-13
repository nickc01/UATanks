using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Extensions;

[RequireComponent(typeof(CharacterController))]
public class TankMover : MonoBehaviour
{
    CharacterController controller;
    private void Start()
    {
        //Gets the character controller of this tank
        controller = GetComponent<CharacterController>();
    }
    //Moves the tank forward at a set speed
    //Negative values move the tank backwards
    public void Move(float speed)
    {
        controller.SimpleMove(transform.forward * speed);
    }

    //Rotates the tank by a set amount of degrees
    //Negative values rotate the tank to the left
    public void Rotate(float degrees)
    {
        transform.Rotate(0, degrees, 0);
    }

    //Rotates towards a specified target with a specific amount of degrees
    public void RotateTowards(Vector3 target, float maxDegrees)
    {
        var angle = GetAngleTo(target);
        if (angle < 0f)
        {
            angle = -Mathf.Clamp(Mathf.Abs(angle),0,maxDegrees);
        }
        else
        {
            angle = Mathf.Clamp(angle,0,maxDegrees);
        }
        Debug.Log("ANGLE = " + angle);
        //Rotate(Mathf.Clamp(angle, 0, maxDegrees));
        Rotate(angle);
    }

    public float GetAngleTo(Vector3 target)
    {
        return Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);
    }
}
