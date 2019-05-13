using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

public class AIEnemyTank : EnemyTank
{
    [Header("Enemy Tank AI Data")]
    [Tooltip("The range at which the enemy can hear the player")]
    [SerializeField] protected float HearingDistance = 6f;
    [Tooltip("The range at which the enemy can see")]
    [SerializeField] protected float SeeingDistance = 6f;
    [Tooltip("The FOV at which the enemy can see the player. Value is in degrees")]
    [SerializeField] protected float SeeingFOV = 45f;
    [Tooltip("The personality of the tank. This will determines the type of AI the tank will use")]
    [SerializeField] private Personality personality;
    [Tooltip("A list of points the enemy tank should patrol. Used in both the patrol and navigate personalities")]
    [SerializeField] private List<Transform> PatrolPoints;
    [Tooltip("How close the tank has to get to a patrol point before it determines that it has reached that point")]
    [SerializeField] private float PatrolMinimumDistance = 0.4f;
    [Tooltip("Determines what the enemy tank will do once it has run out of patrol points in the list")]
    [SerializeField] private PatrolLoopMode patrolLoopMode;
    [Space]
    [Header("DEBUG")]
    [Tooltip("Determines whether to show the FOV or not in the scene")]
    [SerializeField] private bool DebugFOV = false;
    [Tooltip("Determines whether to show the LOS or not in the scene")]
    [SerializeField] private bool DebugLOS = false;
    [Tooltip("Determines whether to show the hearing circle or not")]
    [SerializeField] private bool DebugHearing = false;
    [Tooltip("Determines whether to show the seeing circle or not")]
    [SerializeField] private bool DebugSeeing = false;

    TankHearing Hearing;
    TankVision Vision;

    public override void Start()
    {
        //OA = GetComponent<ObstacleAvoidance>();
        Hearing = GetComponent<TankHearing>();
        Vision = GetComponent<TankVision>();

        Hearing.HearingRange = HearingDistance;
        Vision.SightFOV = SeeingFOV;
        Vision.SightRange = SeeingDistance;

        base.Start();
    }

    protected override void Update()
    {
        if (GameManager.Player.Tank != null)
        {
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
        //Shoot forwards
        Shooter.Shoot();
        //Rotate towards the player, with obstacle avoidance enabled
        Mover.RotateTowards(GameManager.Player.Tank.transform.position, Data.RotateSpeed * Time.deltaTime,true);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Flee from the player
    protected void Flee()
    {
        //Shoot forwards
        Shooter.Shoot();
        //Rotate away from the player, with obstacle avoidance enabled
        Mover.RotateTowards(GameManager.Player.Tank.transform.position, -Data.RotateSpeed * Time.deltaTime, true);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Patrol the area
    protected void Patrol()
    {
        var target = GameManager.Player.Tank.transform.position;
        if (Vision.SeeingTarget(target))
        {
            Shooter.Shoot();
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime);
        }
        else if (Hearing.HearingTarget(target,GameManager.Player.Tank.Noise))
        {
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime);
        }
        else
        {
            Navigate(false);
        }
    }

    private int CurrentPatrolIndex = 0;
    bool pingPongDirection = true;

    //Navigate the area
    protected void Navigate(bool Shoot = true)
    {
        if (Shoot)
        {
            if (Hearing.HearingTarget(GameManager.Player.Tank.transform.position,GameManager.Player.Tank.Noise))
            {
                Shooter.Shoot();
            }
        }
        if (PatrolPoints.Count == 0 || CurrentPatrolIndex == PatrolPoints.Count)
        {
            return;
        }
        Mover.RotateTowards(PatrolPoints[CurrentPatrolIndex].position, Data.RotateSpeed * Time.deltaTime, true);

        Mover.Move(Data.ForwardSpeed);

        if (Vector3.Distance(PatrolPoints[CurrentPatrolIndex].position,transform.position) <= PatrolMinimumDistance)
        {
            switch (patrolLoopMode)
            {
                case PatrolLoopMode.End:
                    CurrentPatrolIndex++;
                    break;
                case PatrolLoopMode.Loop:
                    CurrentPatrolIndex++;
                    if (CurrentPatrolIndex == PatrolPoints.Count)
                    {
                        CurrentPatrolIndex = 0;
                    }
                    break;
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
        if (Health > Data.MaxHealth / 2f)
        {
            Chase();
        }
        else
        {
            Flee();
        }
    }


}
