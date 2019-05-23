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
    private static Dictionary<Type, List<PowerupHolder>> AllPowerups = new Dictionary<Type, List<PowerupHolder>>();

    public static ReadOnlyCollection<PowerupHolder> GetAllPowerups<PowerupType>() where PowerupType : PowerUp
    {
        if (!AllPowerups.ContainsKey(typeof(PowerupType)))
        {
            AllPowerups.Add(typeof(PowerupType), new List<PowerupHolder>());
        }
        return AllPowerups[typeof(PowerupType)].AsReadOnly();
    }

    public static (float Distance, PowerupHolder Holder) GetNearestHolder<T>(Vector3 source) where T : PowerUp
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


    private void Add(Type type)
    {
        if (type == null)
        {
            return;
        }
        if (!AllPowerups.ContainsKey(type))
        {
            AllPowerups.Add(type, new List<PowerupHolder>());
        }
        AllPowerups[type].Add(this);
    }

    private void Remove(Type type)
    {
        if (type == null)
        {
            return;
        }
        if (AllPowerups.ContainsKey(type))
        {
            AllPowerups[type].Remove(this);
            if (AllPowerups[type].Count == 0)
            {
                AllPowerups.Remove(type);
            }
        }
    }

    private void Start()
    {
        Add(powerUp.PowerUpType);
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
                    return;
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

    private void OnDestroy()
    {
        if (Application.isPlaying)
        {
            Remove(powerUp.PowerUpType);
        }
    }
}
