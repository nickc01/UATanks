using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;




public abstract class PowerUp
{
    public PowerUpInfo Info { get; set; }
    public Controller Tank { get; set; }
    public TankData TankData { get; set; }
    public PowerupHolder Holder { get; set; }

    private float timeLeft;

    public float TimeLeft
    {
        get => timeLeft;
        set
        {
            Update();
            if (timeLeft >= Info.WarningTime && value < Info.WarningTime)
            {
                OnWarning();
            }
            timeLeft = value;
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                Destroy();
            }
        }
    }

    public void Activate()
    {
        if (Info.Forever)
        {
            timeLeft = float.PositiveInfinity;
        }
        else
        {
            timeLeft = Info.LifeTime;
        }
        Tank.ActivePowerUps.Add(this);

        GameManager.PlayingLevelEvent += OnLevelEnd;

        OnActivate();
    }

    private void OnLevelEnd(bool playingLevel)
    {
        if (!playingLevel)
        {
            Destroy();
        }
    }

    protected abstract void OnActivate();
    protected abstract void OnWarning();
    protected abstract void OnDeactivate();
    protected virtual void Update() { }


    public void Destroy()
    {
        GameManager.PlayingLevelEvent -= OnLevelEnd;
        OnDeactivate();
        Tank.ActivePowerUps.Remove(this);
        GameObject.Destroy(Holder);
    }
}
