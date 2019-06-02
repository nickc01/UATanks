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
    public bool Moving { get; private set; } = false;
    public float AudioDelay { get; private set; } = 0.05f;
    float SoundTimer = 0f;
    float SoundTimerMax = 0f;

    bool usingMoveCommands = false;


    private void Start()
    {
        //Gets the character controller of this tank
        controller = GetComponent<CharacterController>();
        //Create the Obstacle Avoidance System for this mover
        OA = new ObstacleAvoidance(transform);
    }

    private void LateUpdate()
    {
        Moving = usingMoveCommands;
        //If the tank was moving
        if (Moving)
        {
            //Play the tank sound
            if (GameManager.Game.TankMoveSound != null)
            {
                SoundTimer += Time.deltaTime;
                if (SoundTimerMax == 0)
                {
                    SoundTimerMax = GameManager.Game.TankMoveSound.length;
                }
                if (SoundTimer >= SoundTimerMax)
                {
                    SoundTimer = -AudioDelay;
                    Audio.Play(GameManager.Game.TankMoveSound, () => Audio.SoundEffectsVolume / 3f,transform);
                }
            }
            else
            {
                SoundTimerMax = 0;
            }
        }
        else
        {
            SoundTimer = 0;
        }
        usingMoveCommands = false;
    }

    //Moves the tank forward at a set speed
    //Negative values move the tank backwards
    public void Move(float speed)
    {
        if (speed != 0)
        {
            usingMoveCommands = true;
        }
        controller.SimpleMove(transform.forward * speed);
    }

    //Rotates the tank by a set amount of degrees
    //Negative values rotate the tank to the left
    //If Use OA is set to true, then it will make sure to avoid any obstacles as well
    public void Rotate(float degrees, bool UseOA = false)
    {
        /*if (degrees != 0)
        {
            usingMoveCommands = true;
        }*/
        if (UseOA)
        {
            degrees += degrees * OA.RecommendedDirection;
        }
        transform.Rotate(0, degrees, 0);
    }

    //Rotates towards a specified target with a specific amount of degrees
    //If Use OA is set to true, then it will make sure to avoid any obstacles as well
    public void RotateTowards(Vector3 target, float maxDegrees,bool UseOA = false)
    {
        var angle = 0f;
        //If the angle is negative, then the tank should move away from the target
        if (maxDegrees < 0f)
        {
            //Get the angle to move away from the target
            angle = GetAngleAwayFrom(target);
            //Clamp it below the maxDegrees
            angle = ClampAbs(angle, 0, Mathf.Abs(maxDegrees));
        }
        //If the angle is positive, then the tank should move towards the target
        else
        {
            //Get the angle to move towards the target
            angle = GetAngleTo(target);
            //Clamp it below the maxDgrees
            angle = ClampAbs(angle, 0, Mathf.Abs(maxDegrees));
        }
        //If Obstacle avoidance should be used
        if (UseOA)
        {
            //Rotate either the obstacle avoidance recommended direction or the set angle
            //Pick which one is bigger
            Rotate(BiggerAbsNumber(OA.RecommendedDirection * Mathf.Abs(maxDegrees),angle), false);
        }
        else
        {
            //Rotate the set angle
            Rotate(angle);
        }
    }

    //Gets the amount to rotate to go toward the target
    public float GetAngleTo(Vector3 target)
    {
        return Vector3.SignedAngle(transform.forward, target - transform.position, Vector3.up);
    }

    //Get the amount to rotate to go away from the target
    public float GetAngleAwayFrom(Vector3 target)
    {
        return Vector3.SignedAngle(transform.forward, -(target - transform.position), Vector3.up);
    }
}
