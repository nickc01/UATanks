﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

public class EnemyTank : Controller
{
    /*-----MAIN TANK STATS-----*/

    [Header("Enemy Tank AI Data")]
    [Tooltip("The range at which the enemy can hear the player")]
    [SerializeField] float HearingDistance = 6f;
    [Tooltip("The range at which the enemy can see")]
    [SerializeField] float SeeingDistance = 6f;
    [Tooltip("The FOV at which the enemy can see the player. Value is in degrees")]
    [SerializeField] float SeeingFOV = 45f;
    [Tooltip("Whether to use Obstacle Avoidance in the enemy movement")]
    [SerializeField] bool UseObstacleAvoidance = true;
    [Tooltip("The personality of the tank. This will determines the type of AI the tank will use")]
    [SerializeField] Personality personality;
    [Tooltip("A list of points the enemy tank should patrol. Used in both the patrol and navigate personalities")]
    [SerializeField] List<Transform> PatrolPoints;
    [Tooltip("How close the tank has to get to a patrol point before it determines that it has reached that point")]
    [SerializeField] float PatrolMinimumDistance = 0.4f;
    [Tooltip("Determines what the enemy tank will do once it has run out of patrol points in the list")]
    [SerializeField] PatrolLoopMode patrolLoopMode;

    /*-----DEBUG FLAGS-----*/

    [Header("Enemy Tank DEBUG")]
    [Space]
    [Tooltip("Determines whether to show the FOV or not in the scene")]
    [SerializeField] bool DebugFOV = false;
    [Tooltip("Determines whether to show the hearing circle or not")]
    [SerializeField] bool DebugHearing = false;
    [Tooltip("Determines whether to show the seeing circle or not")]
    [SerializeField] bool DebugSeeing = false;

    /*-----OBSTACLE AVOIDANCE SYSTEM-----*/
    [Header("Obstacle Avoidance System")]
    [Tooltip("The amount of whiskers to use for obstacle avoidance")]
    [SerializeField] private int whiskerAmount = 7;
    [Tooltip("How long the whiskers will be")]
    [SerializeField] private float whiskerLength = 5f;
    [Tooltip("The Field of view the whiskers will cover")]
    [Range(0f, 180f)]
    [SerializeField] private float whiskerFOV = 150f;
    [Tooltip("The layers the whiskers will consider an obstacle")]
    [SerializeField] private LayerMask obstacleLayers;
    [Header("Obstacle Avoidance DEBUG")]
    [Tooltip("Whether to display the whisker lines or not")]
    [SerializeField] private bool debugWhiskers = false;

    TankHearing Hearing; //The hearing component of the enemy tank
    TankVision Vision; //The vision component of the enemy tank
    //ObstacleAvoidance OA; //The obstacle avoidance system of this tank

    public override void Start()
    {
        base.Start();
        //OA = GetComponent<ObstacleAvoidance>();
        Hearing = GetComponent<TankHearing>();
        Vision = GetComponent<TankVision>();

        Hearing.HearingRange = HearingDistance; //Set the hearing range
        Vision.SightFOV = SeeingFOV; //Set the sight FOV
        Vision.SightRange = SeeingDistance; //Set the sight range

        //Add the enemy's data to a list of enemy datas
        GameManager.Enemies.Add((this, Data));

        //Set the stats for the obstacle avoidance system
        var OA = Mover.OA;
        OA.WhiskerAmount = whiskerAmount;
        OA.WhiskerLength = whiskerLength;
        OA.WhiskerFOV = whiskerFOV;
        OA.ObstacleLayers = obstacleLayers;
        OA.Debug = debugWhiskers;

    }

    protected virtual void Update()
    {
        //If there is a player in the scene
        if (GameManager.Player.Tank != null)
        {
            //Use a State machine to determine the action to take based on the personality
            switch (personality)
            {
                case Personality.Chase:
                    Chase();
                    break;
                case Personality.Flee:
                    Flee();
                    break;
                case Personality.Patrol:
                    Patrol();
                    break;
                case Personality.Navigate:
                    Navigate();
                    break;
                case Personality.Strategic:
                    Strategic();
                    break;
            }
        }
        //DEBUG
        if (DebugSeeing)
        {
            DebugDraw.DrawCircle2D(transform.position, SeeingDistance,color: Color.red);
        }
        if (DebugHearing)
        {
            DebugDraw.DrawCircle2D(transform.position, HearingDistance, color: Color.yellow);
        }
        if (DebugFOV)
        {
            DebugDraw.DrawFOV(transform.position, SeeingDistance, transform.eulerAngles.y,SeeingFOV, Color.green);
        }
    }

    //Chase towards the player
    protected void Chase()
    {
        if (Hearing.HearingTarget(GameManager.Player.Tank.transform.position, GameManager.Player.Tank.Noise))
        {
            //Shoot forwards
            Shooter.Shoot();
        }
        //Rotate towards the player, with obstacle avoidance enabled
        Mover.RotateTowards(GameManager.Player.Tank.transform.position, Data.RotateSpeed * Time.deltaTime,true);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Flee from the player
    protected void Flee()
    {
        //TODO : FIX Obstacle Avoidance
        if (Hearing.HearingTarget(GameManager.Player.Tank.transform.position, GameManager.Player.Tank.Noise))
        {
            //Shoot forwards
            Shooter.Shoot();
        }
        //Rotate away from the player, with obstacle avoidance enabled
        Mover.RotateTowards(GameManager.Player.Tank.transform.position, -Data.RotateSpeed * Time.deltaTime, true);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Patrol the area
    protected void Patrol()
    {
        //Set the target to be the player in the scene
        var target = GameManager.Player.Tank.transform.position;
        //If the enemy is able to see the target
        if (Vision.SeeingTarget(target))
        {
            //Shoot at it and rotate towards it
            Shooter.Shoot();
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime);
        }
        //If the enemy can hear the player
        else if (Hearing.HearingTarget(target,GameManager.Player.Tank.Noise))
        {
            //Rotate towards the player
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime,true);
            //Move towards it
            Mover.Move(Data.ForwardSpeed);
        }
        //If the player cannot be seen nor heard
        else
        {
            //Navigate through the patrol points
            Navigate(false);
        }
    }

    private int CurrentPatrolIndex = 0; //The current patrol point in the list that the enemy is heading towards
    bool pingPongDirection = true; //The current ping pong state. If true, then the enemy is going forwards through the patrol list
                                   //If false, then the enemy is going backwards through the patrol list

    //Navigate the area
    protected void Navigate(bool Shoot = true)
    {
        //If shoot mode is enabled
        if (Shoot)
        {
            //If the enemy can hear the player
            if (Hearing.HearingTarget(GameManager.Player.Tank.transform.position,GameManager.Player.Tank.Noise))
            {
                //Shoot forwards
                Shooter.Shoot();
            }
        }
        //If there are no points to patrol, then exit out
        if (PatrolPoints.Count == 0 || CurrentPatrolIndex == PatrolPoints.Count)
        {
            return;
        }

        //Rotate towards the next patrol point
        Mover.RotateTowards(PatrolPoints[CurrentPatrolIndex].position, Data.RotateSpeed * Time.deltaTime, true);

        //Move forwards
        Mover.Move(Data.ForwardSpeed);

        //If the enemy is close enough to the patrol point
        if (Vector3.Distance(PatrolPoints[CurrentPatrolIndex].position,transform.position) <= PatrolMinimumDistance)
        {
            //Determine the next patrol point based on the set patrol loop mode
            switch (patrolLoopMode)
            {
                //If the enemy has reached the last patrol point, then do not move any further
                case PatrolLoopMode.End:
                    CurrentPatrolIndex++;
                    break;
                //If the enemy has reached the last patrol point, then reset back to the start
                case PatrolLoopMode.Loop:
                    CurrentPatrolIndex++;
                    if (CurrentPatrolIndex == PatrolPoints.Count)
                    {
                        CurrentPatrolIndex = 0;
                    }
                    break;
                //If the enemy has reached the last patrol point, then start working backwards
                case PatrolLoopMode.PingPong:
                    if (pingPongDirection == true)
                    {
                        CurrentPatrolIndex++;
                        if (CurrentPatrolIndex == PatrolPoints.Count)
                        {
                            pingPongDirection = false;
                            CurrentPatrolIndex--;
                        }
                    }
                    else
                    {
                        CurrentPatrolIndex--;
                        if (CurrentPatrolIndex < 0)
                        {
                            pingPongDirection = true;
                            CurrentPatrolIndex++;
                        }
                    }
                    break;
            }
        }
    }

    protected void Strategic()
    {
        //If the enemy's health is above half way
        if (Health > Data.MaxHealth / 2f)
        {
            //Then chase the player
            Chase();
        }
        //If the health is less than half
        else
        {
            //Flee from the player
            Flee();
        }
    }

    //When the enemy is hit by a player shell, it reduces the tank's health
    //if the tank's health is zero, the tank is destroyed
    public override void OnShellHit(Shell shell)
    {
        //If the shell came from a player tank
        if (shell.Source is PlayerTank)
        {
            //Decrease the tank's health
            Health -= shell.Damage;
            //Increase source tank's score if health is zero
            if (Health == 0)
            {
                shell.Source.Score += Data.TankValue;
            }
        }
    }

    //Called when the tank dies
    protected override void OnDeath()
    {
        base.OnDeath();
        //Remove this enemy data from the list of enemy data's
        GameManager.Enemies.Remove((this, Data));
        //If there are no enemies in the Enemies list
        if (GameManager.Enemies.Count == 0)
        {
            //Trigger the win condition
            GameManager.Win();
        }
    }


}
