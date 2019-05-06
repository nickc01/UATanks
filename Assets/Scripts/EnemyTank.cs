using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EnemyTank : Controller
{
    public override void Start()
    {
        base.Start();
        //Add the enemy's data to a list of enemy datas
        GameManager.Enemies.Add(Data);
    }

    private void Update()
    {
        //Shoots shells as fast as possible
        //The shoot function has a cooldown timer, so it will limit the amount of shells fired based on the cooldown
        Shooter.Shoot(Data.ShellSpeed, Data.ShellDamage, Data.ShellLifetime);
    }

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

    protected override void OnDeath()
    {
        base.OnDeath();
        //Remove this enemy data from the list of enemy data's
        GameManager.Enemies.Remove(Data);
    }

}