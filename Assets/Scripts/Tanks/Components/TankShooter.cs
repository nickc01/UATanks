using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class TankShooter : MonoBehaviour
{
    private Tank controller; //The tank controller of this object
    private TankData data; //The tank data of this object
    public float FireRate { get; set; } = 0f; //The cooldown timer before another shell can be fired
    public bool Firing { get; private set; } = false; //Whether the tank is shooting a bullet or not

    private float CooldownTracker = 0f; //A clock for determining how long the cooldown will take

    private void Start()
    {
        //Get the tank controller
        controller = GetComponent<Tank>();
        //Get the tank data
        data = GetComponent<TankData>();
    }

    private void Update()
    {
        //If the shooter is firing
        if (Firing)
        {
            //Decrease the cooldown tracker
            CooldownTracker -= Time.deltaTime;
            //If the cooldown is over
            if (CooldownTracker <= 0)
            {
                //The shooter is ready to fire another bullet
                Firing = false;
            }
        }
    }

    //Shoots a shell with a set speed, damage, and lifetime
    public void Shoot(float Speed, float Damage, float Lifetime)
    {
        //If the firing cooldown is still present, then do not fire a shell
        if (!Firing)
        {
            //Reset the cooldown tracker
            CooldownTracker = FireRate;
            //Otherwise, create a new shell object and set its stats
            Firing = true;
            var newShell = Instantiate(Game.ShellPrefab, transform.position, transform.rotation).GetComponent<Shell>();
            newShell.Set(Lifetime, Damage, Speed, controller,1f);
        }
    }
    //Shoots a shell with a set speed, damage, and lifetime
    //Retrieves the values from the tank's data
    public void Shoot()
    {
        Shoot(data.ShellSpeed, data.ShellDamage, data.ShellLifetime);
    }
}
