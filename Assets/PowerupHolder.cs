using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


public class PowerupHolder : MonoBehaviour
{
    public PowerUpInfo powerUp;

    public bool Activated { get; private set; } = false;

    private bool visible = true;

    public bool Visible
    {
        get => visible;
        set
        {
            if (visible != value)
            {
                visible = value;
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = value;
                }
            }
        }
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (!Activated)
        {
            var controller = other.GetComponent<Controller>();
            if (controller != null && controller.Data != null)
            {
                //Get the powerup type
                Type powerupType = powerUp.PowerUpType;
                if (powerupType == null)
                {
                    Destroy(gameObject);
                }
                Activated = true;
                //Instantiate the new powerup via the Activator class
                var NewPowerUp = Activator.CreateInstance(powerupType) as PowerUp;
                NewPowerUp.Info = powerUp;
                NewPowerUp.Tank = controller;
                NewPowerUp.TankData = controller.Data;
                NewPowerUp.Holder = this;

                transform.parent = controller.transform;
                transform.localPosition = Vector3.zero;
                Visible = false;

                if (powerUp.OneAtATime)
                {
                    var finding = controller.ActivePowerUps.FirstOrDefault(p => p.GetType() == powerupType);
                    if (finding != null)
                    {
                        //Deactivate it
                        finding.Destroy();
                    }
                }

                NewPowerUp.Activate();
            }
        }
    }
}
