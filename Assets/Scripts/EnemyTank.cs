using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyTank : Controller
{
    [SerializeField] float HearingRadius = 6f; //The range at which the enemy can hear the player

    public override void Start()
    {
        base.Start();
        //Add the enemy's data to a list of enemy datas
        GameManager.Enemies.Add((this,Data));
    }

    private void Update()
    {
        //Shoots shells as fast as possible
        //The shoot function has a cooldown timer, so it will limit the amount of shells fired based on the cooldown
        Shooter.Shoot(Data.ShellSpeed, Data.ShellDamage, Data.ShellLifetime);
        //If there is a player in the game and the player is within hearing radius
        if (GameManager.Player.Tank != null && Vector3.Distance(GameManager.Player.Tank.transform.position,transform.position) < HearingRadius)
        {
            var playerPosition = GameManager.Player.Tank.transform.position;
            var angle = Vector3.SignedAngle(playerPosition - transform.position, transform.forward, Vector3.up);
            if (angle < 0)
            {
                Mover.Rotate(Mathf.Clamp(-angle, 0, Data.RotateSpeed * Time.deltaTime));
            }
            else if (angle > 0)
            {
                Mover.Rotate(Mathf.Clamp(-angle, -Data.RotateSpeed * Time.deltaTime, 0));
            }
        }
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
        GameManager.Enemies.Remove((this,Data));
        //If there are no enemies in the Enemies list
        if (GameManager.Enemies.Count == 0)
        {
            //Trigger the win condition
            GameManager.Win();
        }
    }

}