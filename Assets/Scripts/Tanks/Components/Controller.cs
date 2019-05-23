using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMover), typeof(TankShooter), typeof(TankData))]
public abstract class Controller : MonoBehaviour, IOnShellHit
{
    protected TankMover Mover; //The mover component for the tank
    protected TankShooter Shooter; //The shooter component of the tank
    public TankData Data { get; protected set; } //The data of the tank
    [HideInInspector]
    public List<PowerUp> ActivePowerUps = new List<PowerUp>();

    public virtual float Health //The health of the tank
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
    public abstract bool OnShellHit(Shell shell);

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

        //Set the color of any colorizers on this object
        foreach (var colorizer in GetComponentsInChildren<TankColorer>())
        {
            colorizer.Color = Data.color;
        }
    }

    public virtual void Update()
    {
        for (int i = ActivePowerUps.Count - 1; i >= 0; i--)
        {
            ActivePowerUps[i].TimeLeft -= Time.deltaTime;
        }
    }

    //Called when the tank's health is zero
    protected virtual void OnDeath()
    {
        //Deactivate all the active powerups
        for (int i = ActivePowerUps.Count - 1; i >= 0; i--)
        {
            ActivePowerUps[i].Destroy();
        }
        //Destroy the tank
        Destroy(gameObject);
    }
}
