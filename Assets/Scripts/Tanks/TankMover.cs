using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
