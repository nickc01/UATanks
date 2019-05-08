using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Controller used for the player tank
//Primarilly handles inputs and moves the tank depending on said inputs
public class PlayerTank : Controller
{
    public override void Start()
    {
        base.Start();
        //Set the main player data
        GameManager.Player = (this, Data);
        //Set the camera target to be the player tank
        CameraController.Target = gameObject;
    }

    //The Score for the player
    public override float Score
    {
        get => base.Score;
        set
        {
            base.Score = value;
            //Update the score display
            ScoreDisplay.Score = value;
        }
    }

    //Used to control input
    private void Update()
    {
        //If the spacebar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            //Shoot a shell
            Shooter.Shoot(Data.ShellSpeed, Data.ShellDamage, Data.ShellLifetime);
        }
        //If the W or Up Arrow Keys are currently held down
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            //Move the tank forward
            Mover.Move(Data.ForwardSpeed);
        }
        //If the S or Down Arrow Keys are currently held down
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            //Move the tank backwards
            Mover.Move(-Data.BackwardSpeed);
        }
        //If neither the up or down inputs are pressed
        else
        {
            //Do not move the tank and just apply gravity
            Mover.Move(0);
        }
        //If the A or Left Arrow Keys are currently held down
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            //Rotate the tank to the left
            Mover.Rotate(-Data.RotateSpeed * Time.deltaTime);
        }
        //If the D or Right Arrow Keys are currently held down
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            //Rotate the tank to the right
            Mover.Rotate(Data.RotateSpeed * Time.deltaTime);
        }
    }


    public override void OnShellHit(Shell shell)
    {
        //If the shell came from a enemy tank
        if (shell.Source is EnemyTank)
        {
            //Decrease the tank's health
            Health -= shell.Damage;
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        //Set the main player to null
        GameManager.Player = (null, null);
        //Trigger the lose condition
        GameManager.Lose();
    }
}