using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyTank : Controller
{

    public override void Start()
    {
        base.Start();
        //Add the enemy's data to a list of enemy datas
        GameManager.Enemies.Add((this,Data));
    }

    protected virtual void Update()
    {

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
        GameManager.Enemies.Remove((this,Data));
        //If there are no enemies in the Enemies list
        if (GameManager.Enemies.Count == 0)
        {
            //Trigger the win condition
            GameManager.Win();
        }
    }

}