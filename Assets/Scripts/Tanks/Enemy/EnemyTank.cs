using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyTank : Tank
{
    /*-----MAIN TANK STATS-----*/

    [Header("Enemy Tank General Data")]
    [Tooltip("The range at which the enemy can hear the player")]
    [SerializeField] float HearingDistance = 6f;
    [Tooltip("The range at which the enemy can see")]
    [SerializeField] float SeeingDistance = 6f;
    [Tooltip("The FOV at which the enemy can see the player. Value is in degrees")]
    [SerializeField] float SeeingFOV = 45f;
    [Tooltip("A list of object types that will block the enemy's line of sight")]
    [SerializeField] LayerMask SightObstacles = default;
    [Tooltip("Whether to use Obstacle Avoidance in the enemy movement")]
    [SerializeField] bool UseObstacleAvoidance = true;
    [Tooltip("The personality of the tank. This will determines the type of AI the tank will use")]
    [PropSender("Patrolling","personality",Personality.Patrol)]
    [SerializeField] Personality personality = Personality.Chase;
    [Tooltip("A list of points the enemy tank should patrol. Used in both the patrol and navigate personalities")]
    [PropReceiver("Patrolling")]
    [SerializeField] List<Transform> patrolPoints = new List<Transform>();
    [Tooltip("How close the tank has to get to a patrol point before it determines that it has reached that point")]
    [PropReceiver("Patrolling")]
    [SerializeField] float PatrolMinimumDistance = 0.4f;
    [Tooltip("Determines what the enemy tank will do once it has run out of patrol points in the list")]
    [PropReceiver("Patrolling")]
    [SerializeField] PatrolLoopMode patrolLoopMode = PatrolLoopMode.End;

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
    [SerializeField] private LayerMask obstacleLayers = default;
    [Header("Obstacle Avoidance DEBUG")]
    [Tooltip("Whether to display the whisker lines or not")]
    [SerializeField] private bool debugWhiskers = false;

    Hearing Hearing; //The hearing component of the enemy tank
    Vision Vision; //The vision component of the enemy tank
    TankHealthDisplay tankHealth; //The component to display the tank's health

    //Public Interfaces
    public Personality Personality => personality;
    public List<Transform> PatrolPoints => patrolPoints;

    public override void Start()
    {
        base.Start();

        //Create the hearing and sight components
        Hearing = new Hearing(transform, HearingDistance);
        Vision = new Vision(transform, SeeingDistance, SeeingFOV, SightObstacles);

        //Get the tank health component
        tankHealth = GetComponentInChildren<TankHealthDisplay>();

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

    //Gets and sets the health of the enemy tank
    public override float Health
    {
        get => base.Health;
        set
        {
            base.Health = value;
            //Update the tank health display
            tankHealth.Health = value / Data.MaxHealth;
        }
    }

    public override void Update()
    {
        base.Update();
        var Player = GameManager.GetNearestPlayer(this);
        //If there is a player in the scene
        if (!Dead && Player.Tank != null && GameManager.PlayingLevel && Vector3.Distance(Player.Tank.transform.position,transform.position) < 75f)
        {
            //Use a State machine to determine the action to take based on the personality
            switch (personality)
            {
                case Personality.Chase:
                    Chase(Player.Tank); //Run the chase AI state
                    break;
                case Personality.Flee:
                    Flee(Player.Tank); //Run the flee AI state
                    break;
                case Personality.Patrol:
                    Patrol(Player.Tank); //Run the patrol AI state
                    break;
                case Personality.Navigate:
                    Navigate(Player.Tank); //Run the navigate AI state
                    break;
                case Personality.Strategic:
                    Strategic(Player.Tank); //Run the strategic AI state
                    break;
            }
        }
        //DEBUG
        if (DebugSeeing)
        {
            //Draw a circle representing the sight radius
            DebugDraw.DrawCircle2D(transform.position, SeeingDistance,color: Color.red);
        }
        if (DebugHearing)
        {
            //Draw a circle representing the hearing radius
            DebugDraw.DrawCircle2D(transform.position, HearingDistance, color: Color.yellow);
        }
        if (DebugFOV)
        {
            //Draw the sight FOV
            DebugDraw.DrawFOV(transform.position, SeeingDistance, transform.eulerAngles.y,SeeingFOV, Color.green);
        }
    }

    //Chase towards the player
    protected void Chase(PlayerTank Player,Transform target = null)
    {
        target = target ?? Player.transform;
        //If the tank can hear the player
        if (Hearing.CanHearTarget(target.position, Player.Noise))
        {
            //Shoot forwards
            Shooter.Shoot();
        }
        //Rotate towards the player
        Mover.RotateTowards(target.transform.position, Data.RotateSpeed * Time.deltaTime,UseObstacleAvoidance);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Flee from the player
    protected void Flee(PlayerTank Player)
    {
        //If the tank can hear the player
        if (Hearing.CanHearTarget(Player.transform.position, Player.Noise))
        {
            //Shoot forwards
            Shooter.Shoot();
        }
        //Rotate away from the player, with obstacle avoidance enabled
        Mover.RotateTowards(Player.transform.position, -Data.RotateSpeed * Time.deltaTime, UseObstacleAvoidance);
        //Move forward
        Mover.Move(Data.ForwardSpeed);
    }

    //Patrol the area
    protected void Patrol(PlayerTank Player)
    {
        //Set the target to be the player in the scene
        var target = Player.transform.position;
        //If the enemy is able to see the target
        if (Vision.CanSeeTarget(target))
        {
            //Shoot at it and rotate towards it
            Shooter.Shoot();
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime);
        }
        //If the enemy can hear the player
        else if (Hearing.CanHearTarget(target,Player.Noise))
        {
            //Rotate towards the player
            Mover.RotateTowards(target, Data.RotateSpeed * Time.deltaTime,UseObstacleAvoidance);
            //Move towards it
            Mover.Move(Data.ForwardSpeed);
        }
        //If the player cannot be seen nor heard
        else
        {
            //Navigate through the patrol points
            Navigate(Player,false);
        }
    }

    private int CurrentPatrolIndex = 0; //The current patrol point in the list that the enemy is heading towards
    bool pingPongDirection = true; //The current ping pong state. If true, then the enemy is going forwards through the patrol list
                                   //If false, then the enemy is going backwards through the patrol list

    //Navigate the area
    protected void Navigate(PlayerTank Player, bool Shoot = true)
    {
        //If shoot mode is enabled
        if (Shoot)
        {
            //If the enemy can hear the player
            if (Hearing.CanHearTarget(Player.transform.position,Player.Noise))
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
        Mover.RotateTowards(PatrolPoints[CurrentPatrolIndex].position, Data.RotateSpeed * Time.deltaTime, UseObstacleAvoidance);

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
                //If the loop mode is ping-pong and is currently going forward through the list
                case PatrolLoopMode.PingPong when pingPongDirection == true:
                    //Increase the index
                    CurrentPatrolIndex++;
                    //If the index is at the end of the list
                    if (CurrentPatrolIndex == PatrolPoints.Count)
                    {
                        //Start going backwards through the list and set the index to the last patrol point in the list
                        pingPongDirection = false;
                        CurrentPatrolIndex--;
                    }
                    break;
                //If the loop mode is ping-pong and is currently going backwards through the list
                case PatrolLoopMode.PingPong when pingPongDirection == false:
                    //Reduce the index
                    CurrentPatrolIndex--;
                    //If the index is less than zero
                    if (CurrentPatrolIndex < 0)
                    {
                        //Start going forward through the list and set the index to zero
                        pingPongDirection = true;
                        CurrentPatrolIndex++;
                    }
                    break;
            }
        }
    }

    protected void Strategic(PlayerTank Player)
    {
        //If the enemy's health is above half way
        if (Health > Data.MaxHealth / 2f)
        {
            //Then chase the player
            Chase(Player);
        }
        //If the health is less than half
        else
        {
            //Get the nearest health powerup
            var (_, powerup) = PowerupHolder.GetNearestPowerup<HealthPowerup>(transform.position);
            //If there is a powerup
            if (powerup != null)
            {
                //Move to it
                Chase(Player,powerup.transform);
            }
            else
            {
                //Flee from the player
                Flee(Player);
            }
        }
    }

    //When the enemy is hit by a player shell, it reduces the tank's health
    //if the tank's health is zero, the tank is destroyed
    public override bool OnShellHit(Shell shell)
    {
        if (shell.Source == this || Dead)
        {
            return false;
        }
        //If the shell came from a player tank
        if (shell.Source is PlayerTank)
        {
            Attack(shell.Damage);
            //Increase source tank's score if health is zero
            if (Health == 0)
            {
                shell.Source.Score += Data.TankValue;
            }
        }
        return true;
    }

    //Called when the tank dies
    protected override void OnDeath()
    {
        base.OnDeath();
        if (Lives == 0)
        {
            //Remove this enemy data from the list of enemy data's
            GameManager.Enemies.Remove((this, Data));
            //If there are no enemies in the Enemies list
            if (GameManager.Enemies.Count == 0 && GameManager.PlayingLevel && GameManager.Players.Count == 1)
            {
                //Trigger the win condition
                GameManager.Win(GameManager.Players[0].Tank);
            }
        }
    }


}
