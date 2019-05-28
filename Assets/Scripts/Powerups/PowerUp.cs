using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;




public abstract class PowerUp
{
    public PowerUpInfo Info { get; set; } //The configurable info of the powerup
    public Tank Tank { get; set; } //The source tank
    public TankData TankData { get; set; } //The source tank's data
    public PowerupHolder Holder { get; set; } //The holder that spawned the powerup

    private float timeLeft; //The amount of time left before the powerup is destroyed

    public float TimeLeft
    {
        get => timeLeft;
        set
        {
            Update(); //Call the update function
            //If the time remaining goes below the warning time
            if (timeLeft >= Info.WarningTime && value < Info.WarningTime)
            {
                //Trigger the warning signal
                OnWarning();
            }
            timeLeft = value;
            //If there is no more time left
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                //Destroy the powerup
                Destroy();
            }
        }
    }

    //Called when the powerup is activated
    public void Activate()
    {
        //If the powerup will last forever
        if (Info.Forever)
        {
            //Set the time to be infinite
            timeLeft = float.PositiveInfinity;
        }
        else
        {
            //Otherwise, set the time to be the set lifetime
            timeLeft = Info.LifeTime;
        }
        //Add this powerup to the tank's list of powerups
        Tank.ActivePowerUps.Add(this);

        //Add a function that is called when the level ends
        GameManager.PlayingLevelEvent += OnLevelEnd;
        //Call the on activate function
        OnActivate();
    }

    //Called when the level ends
    private void OnLevelEnd(bool playingLevel)
    {
        //If the game is no longer being played
        if (!playingLevel)
        {
            //Destroy the powerup
            Destroy();
        }
    }

    protected abstract void OnActivate(); //Called when the powerup is activated
    protected abstract void OnWarning(); //Called when the powerup is about to run out
    protected abstract void OnDeactivate(); //Called when the powerup has run out
    protected virtual void Update() { } //Called each frame

    //A function to destroy the powerup
    public void Destroy()
    {
        //Remove the function that is called when the level ends
        GameManager.PlayingLevelEvent -= OnLevelEnd;
        //Deactivate the powerup
        OnDeactivate();
        //Remove this powerup from the tank's list of powerups
        Tank.ActivePowerUps.Remove(this);
        //Destroy the original holder
        if (Holder != null)
        {
            GameObject.Destroy(Holder);
        }
    }
}
