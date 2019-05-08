﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMover), typeof(TankShooter), typeof(TankData))]
public abstract class Controller : MonoBehaviour, IOnShellHit
{
    protected TankMover Mover; //The mover component for the tank
    protected TankShooter Shooter; //The shooter component of the tank
    protected TankData Data; //The data of the tank

    public float Health //The health of the tank
    {
        get => Data.Health;
        set
        {
            //Set the health and clamp it within the range of 0 - MaxHealth
            Data.Health = Mathf.Clamp(value, 0, Data.MaxHealth);
            //If the health of the tank is zero
            if (Data.Health == 0)
            {
                //Destroy the tank
                OnDeath();
            }
        }
    }
    public virtual float Score { get => Data.Score; set => Data.Score = value; } //A public accessor for the score

    //When the tank is hit by a shell
    public abstract void OnShellHit(Shell shell);

    public virtual void Start()
    {
        //Get the main tank components
        Mover = GetComponent<TankMover>();
        Shooter = GetComponent<TankShooter>();
        Data = GetComponent<TankData>();
        //Reset the tank's health
        Data.Health = Data.MaxHealth;
        //Set the firing rate
        Shooter.FireRate = Data.FireRate;
    }

    //Called when the tank's health is zero
    protected virtual void OnDeath()
    {
        //Destroy the tank
        Destroy(gameObject);
    }
}