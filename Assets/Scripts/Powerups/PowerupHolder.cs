using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


public class PowerupHolder : MonoBehaviour
{

    //Gets all the powerups of a specific type
    public static ReadOnlyCollection<PowerupHolder> GetAllPowerups<PowerupType>() where PowerupType : PowerUp
    {
        if (!GameManager.AllPowerups.ContainsKey(typeof(PowerupType)))
        {
            GameManager.AllPowerups.Add(typeof(PowerupType), new List<PowerupHolder>());
        }
        return GameManager.AllPowerups[typeof(PowerupType)].AsReadOnly();
    }

    //Gets the nearest powerup of a specific type
    public static (float Distance, PowerupHolder Holder) GetNearestPowerup<T>(Vector3 source) where T : PowerUp
    {
        (float Distance, PowerupHolder Holder) Nearest = (float.PositiveInfinity, null);
        foreach (var holder in GetAllPowerups<T>())
        {
            var distance = Vector3.Distance(source, holder.transform.position);
            if (distance < Nearest.Distance)
            {
                Nearest = (distance, holder);
            }
        }
        return Nearest;
    }



    public PowerUpInfo powerUp; //The powerup info

    public bool Activated { get; private set; } = false; //Whether this powerup holder is activated

    private bool visible = true; //Whether this holder is visible

    public bool Visible //The public interface for accessing the visibility of the holder
    {
        get => visible;
        set
        {
            if (visible != value)
            {
                visible = value;
                //Shows or hides the powerup holder based on the value set
                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = value;
                }
            }
        }
    }


    //Adds a powerup to the list
    private void Add(Type type)
    {
        if (type == null)
        {
            return;
        }
        if (!GameManager.AllPowerups.ContainsKey(type))
        {
            GameManager.AllPowerups.Add(type, new List<PowerupHolder>());
        }
        GameManager.AllPowerups[type].Add(this);
    }

    //Removes a powerup from the list
    private void Remove(Type type)
    {
        if (type == null)
        {
            return;
        }
        if (GameManager.AllPowerups.ContainsKey(type))
        {
            GameManager.AllPowerups[type].Remove(this);
            if (GameManager.AllPowerups[type].Count == 0)
            {
                GameManager.AllPowerups.Remove(type);
            }
        }
    }

    //Adds itself to the list
    private void Start()
    {
        Add(powerUp.PowerUpType);
    }


    protected void OnTriggerEnter(Collider other)
    {
        if (!Activated)
        {
            //If the powerup holder has collided with a tank
            var controller = other.GetComponent<Tank>();
            if (controller != null && controller.Data != null)
            {
                //Get the powerup type
                Type powerupType = powerUp.PowerUpType;
                if (powerupType == null)
                {
                    Destroy(gameObject);
                    return;
                }
                Activated = true;
                //Instantiate the new powerup via the Activator class
                var NewPowerUp = Activator.CreateInstance(powerupType) as PowerUp;
                //Set the powerup's stats
                NewPowerUp.Info = powerUp;
                NewPowerUp.Tank = controller;
                NewPowerUp.TankData = controller.Data;
                NewPowerUp.Holder = this;

                //Set the holder to follow the player
                transform.parent = controller.transform;
                transform.localPosition = Vector3.zero;
                //Hide the holder
                Visible = false;

                //If the powerup can only be activated one at a time
                if (powerUp.OneAtATime)
                {
                    //Disable the one that is already active
                    var finding = controller.ActivePowerUps.FirstOrDefault(p => p.GetType() == powerupType);
                    if (finding != null)
                    {
                        //Deactivate it
                        finding.Destroy();
                    }
                }
                Audio.Play(GameManager.Game.PowerupPickupSound, Audio.SoundEffects,transform);
                NewPowerUp.Activate();
            }
        }
    }

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            //Removes itself from the list
            Remove(powerUp.PowerUpType);
        }
    }
}
