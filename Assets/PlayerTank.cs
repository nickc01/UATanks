using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTank : Controller
{

    public override void Start()
    {
        base.Start();
        Shooter.FireRate = Data.FireRate;
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
        //If the W Key is currently held down
        if (Input.GetKey(KeyCode.W))
        {
            //Move the tank forward
            Mover.Move(Data.ForwardSpeed);
        }
        //If the S Key is currently held down
        else if (Input.GetKey(KeyCode.S))
        {
            //Move the tank backwards
            Mover.Move(-Data.BackwardSpeed);
        }
        else
        {
            Mover.Move(0);
        }
        //If the S Key is currently held down
        if (Input.GetKey(KeyCode.A))
        {
            //Rotate the tank to the left
            Mover.Rotate(-Data.RotateSpeed * Time.deltaTime);
        }
        //If the D Key is currently held down
        if (Input.GetKey(KeyCode.D))
        {
            //Rotate the tank to the right
            Mover.Rotate(Data.RotateSpeed * Time.deltaTime);
        }
    }


    public override void OnShellHit(Shell shell)
    {
        throw new System.NotImplementedException();
    }
}