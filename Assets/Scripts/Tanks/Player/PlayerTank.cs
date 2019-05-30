using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Controller used for the player tank
//Primarilly handles inputs and moves the tank depending on said inputs
public class PlayerTank : Tank
{
    public float Noise { get; private set; } //The amound of audio noise the player is emitting.
                                             //The higher the number, the easier the player can be heard by the enemies

    public PlayerInfo Info => MultiplayerManager.GetPlayerInfo(PlayerNumber);
    public UIManager UI => Info.PlayerUI;

    ControlScheme CurrentScheme;

    public override void Start()
    {
        base.Start();
        //Set the main player data
        //GameManager.Player = (this, Data);
        GameManager.Players.Add((this, Data));
        CurrentScheme = ControlScheme.GetScheme(PlayerNumber);
        //Set the camera target to be the player tank
        Info.PlayerCamera.Target = gameObject;
        //CameraController.Target = gameObject;
        AudioPlayer.Listeners.Add(transform);
        Health = Data.MaxHealth;
        Lives = Data.MaxLives;
        Score = 0;
    }

    //The health of the player
    public override float Health
    {
        get => base.Health;
        set => Info.HealthDisplay.Value = (base.Health = value) / Data.MaxHealth;
    }

    //The Score for the player
    public override float Score
    {
        get => base.Score;
        set => Info.ScoreDisplay.Value = base.Score = value;
    }

    public override int Lives
    {
        get => base.Lives;
        set => Info.LivesDisplay.Value = base.Lives = value;
    }

    public int PlayerNumber { get; set; } = 1;

    //Used to control input
    public override void Update()
    {
        base.Update();
        Noise = 0;
        if (!Dead && GameManager.PlayingLevel)
        {
            //If the spacebar is pressed
            if (CurrentScheme.Firing)
            {
                //Shoot a shell
                Shooter.Shoot();
                Noise += 3f;
            }
            //If the W or Up Arrow Keys are currently held down
            if (CurrentScheme.MovingForward)
            {
                //Move the tank forward
                Mover.Move(Data.ForwardSpeed);
                Noise += 3f;
            }
            //If the S or Down Arrow Keys are currently held down
            else if (CurrentScheme.MovingBackward)
            {
                //Move the tank backwards
                Mover.Move(-Data.BackwardSpeed);
                Noise += 3f;
            }
            //If neither the up or down inputs are pressed
            else
            {
                //Do not move the tank and just apply gravity
                Mover.Move(0);
            }
            //If the A or Left Arrow Keys are currently held down
            if (CurrentScheme.MovingLeft)
            {
                //Rotate the tank to the left
                Mover.Rotate(-Data.RotateSpeed * Time.deltaTime);
            }
            //If the D or Right Arrow Keys are currently held down
            if (CurrentScheme.MovingRight)
            {
                //Rotate the tank to the right
                Mover.Rotate(Data.RotateSpeed * Time.deltaTime);
            }
        }
    }


    public override bool OnShellHit(Shell shell)
    {
        if (shell.Source == this || Dead)
        {
            return false;
        }
        //If the shell came from a enemy tank
        if (shell.Source is Tank)
        {
            //Decrease the tank's health
            Attack(shell.Damage);
            if (Health == 0)
            {
                shell.Source.Score += Data.TankValue;
            }
        }
        return true;
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        //Trigger the lose condition
        if (GameManager.PlayingLevel && Lives == 0)
        {
            //Set the main player to null
            //GameManager.Player = (null, null);
            GameManager.Players.Remove((this,Data));
            AudioPlayer.Listeners.Remove(transform);
            GameManager.Lose(this);
        }
    }

    public static PlayerTank Create(Vector3 spawnPoint,int playerID,Quaternion? Rotation = null)
    {
        var newPlayer = Instantiate(GameManager.Game.PlayerPrefab, spawnPoint, Rotation.GetValueOrDefault(Quaternion.identity)).GetComponent<PlayerTank>();
        newPlayer.PlayerNumber = playerID;
        return newPlayer;
    }
}