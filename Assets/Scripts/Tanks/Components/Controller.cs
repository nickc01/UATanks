using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[RequireComponent(typeof(TankMover), typeof(TankShooter), typeof(TankData))]
public abstract class Tank : MonoBehaviour, IOnShellHit
{
    public static List<Tank> AllTanks = new List<Tank>();

    protected TankMover Mover; //The mover component for the tank
    protected TankShooter Shooter; //The shooter component of the tank
    public TankData Data { get; protected set; } //The data of the tank
    [HideInInspector]
    public List<PowerUp> ActivePowerUps = new List<PowerUp>();

    public Vector3 Spawnpoint { get; private set; } //The place the tank spawned at
    public bool Dead { get; private set; } = false; //Whether the tank is dead or not

    private ReadOnlyCollection<Renderer> TankRenderers;
    Coroutine Respawner;

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

    private bool visible = true;
    public bool Visible
    {
        get => visible;
        set
        {
            if (visible != value)
            {
                visible = value;
                foreach (var renderer in TankRenderers)
                {
                    renderer.enabled = value;
                }
            }
        }
    }

    public virtual float Score { get => Data.Score; set => Data.Score = value; } //A public accessor for the score
    public virtual int Lives { get => Data.Lives; set => Data.Lives = value; } //A public accessor for the lives

    //When the tank is hit by a shell
    public abstract bool OnShellHit(Shell shell);

    public virtual void Start()
    {
        TankRenderers = new ReadOnlyCollection<Renderer>(GetComponentsInChildren<Renderer>());
        Spawnpoint = transform.position;
        //Get the main tank components
        Mover = GetComponent<TankMover>();
        Shooter = GetComponent<TankShooter>();
        Data = GetComponent<TankData>();
        //Reset the tank's health
        Data.Health = Data.MaxHealth;
        //Set the firing rate
        Shooter.FireRate = Data.FireRate;

        AllTanks.Add(this);

        //Set the color of any colorizers on this object
        foreach (var colorizer in GetComponentsInChildren<TankColorer>())
        {
            colorizer.Color = Data.color;
        }
    }

    public virtual void Update()
    {
        //Decrease the powerup timers
        for (int i = ActivePowerUps.Count - 1; i >= 0; i--)
        {
            ActivePowerUps[i].TimeLeft -= Time.deltaTime;
        }
    }

    public void Attack(float Damage)
    {
        //Decrease the tank's health
        Health -= Mathf.Clamp(Damage - Data.DamageResistance, 0f, Damage);
    }

    //Called when the tank's health is zero
    protected virtual void OnDeath()
    {
        if (Dead)
        {
            return;
        }
        Dead = true;
        //Deactivate all the active powerups
        for (int i = ActivePowerUps.Count - 1; i >= 0; i--)
        {
            ActivePowerUps[i].Destroy();
        }
        Lives--;
        if (Lives == 0)
        {
            //Destroy the tank
            AllTanks.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            if (Respawner != null)
            {
                StopCoroutine(Respawner);
            }
            Respawner = StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        Visible = false;
        yield return new WaitForSeconds(Data.RespawnDelay);
        transform.position = Spawnpoint;
        Visible = true;
        new Invincibility(this, Data.RespawnInvincibility);
        Dead = false;
    }
}
