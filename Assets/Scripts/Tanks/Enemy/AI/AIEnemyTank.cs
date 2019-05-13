using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

[RequireComponent(typeof(ObstacleAvoidance))]
public class AIEnemyTank : EnemyTank
{
    [Header("Enemy Tank AI Data")]
    [Tooltip("The range at which the enemy can hear the player")]
    [SerializeField] protected float HearingDistance = 6f;
    [Tooltip("The range at which the enemy can see")]
    [SerializeField] protected float SeeingDistance = 6f;
    [Tooltip("The FOV at which the enemy can see the player. Value is in degrees")]
    [SerializeField] protected float SeeingFOV = 45f;
    [Tooltip("The FOV that determines if the enemy is looking directly at the player. Value is in degrees")]
    [SerializeField] protected float SeeingLOS = 5f;
    [Tooltip("The personality of the tank. This will determines the type of AI the tank will use")]
    [SerializeField] private Personality personality;
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

    //private ObstacleAvoidance OA;

    public override void Start()
    {
        //OA = GetComponent<ObstacleAvoidance>();
        base.Start();
    }

    protected override void Update()
    {
        Debug.Log("UPDATE");
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
        if (DebugLOS)
        {
            DebugDraw.DrawFOV(transform.position, SeeingDistance, transform.eulerAngles.y, SeeingLOS, Color.blue);
        }
    }

    //Chase towards the player
    protected void Chase()
    {
        //Rotate towards the player, with obstacle avoidance enabled
        Mover.RotateTowards(GameManager.Player.Tank.transform.position, Data.RotateSpeed * Time.deltaTime,true);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Flee from the player
    protected void Flee()
    {

    }

    //Patrol the area
    protected void Patrol()
    {

    }

    //Navigate the area
    protected void Navigate()
    {

    }


}
