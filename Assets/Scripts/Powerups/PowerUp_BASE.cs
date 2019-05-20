using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public abstract class PowerUp : MonoBehaviour
{
    [Header("Powerup Stats")]
    [Tooltip("Whether the powerup will last forever or not")]
    [HiddenSender("Forever", false)]
    [SerializeField] protected bool Forever = false;
    [Tooltip("How long the powerup will last")]
    [HiddenReceiver("Forever")]
    [SerializeField] protected float LifeTime = 9f;
    [HiddenReceiver("Forever")]
    [Tooltip("The warning time used to warn the player when the powerup is about to run out")]
    [SerializeField] protected float WarningTime = 3f;
    [Tooltip("How long it takes for the powerup to spawn in the game")]
    public Vector2 SpawnTimeMinMax;
    [Tooltip("If true, will make sure that tanks cannot have more than one of this powerup active on them at once")]
    [SerializeField] protected bool OneAtATime = false;

    public bool Activated { get; private set; } = false;

    public Controller Tank { get; private set; }
    public TankData TankData { get; private set; }

    bool visible = true;

    protected bool Visible
    {
        get => visible;
        set
        {
            visible = value;
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = value;
            }
        }
    }

    //Called when the powerup is activated
    //Base method makes the object invisible and starts the timer to keep track of the lifetime
    public virtual void OnActivated(TankData tankData, Controller tankController)
    {
        if (OneAtATime)
        {
            //If a powerup of the same type is already active
            var finding = tankController.ActivePowerUps.FirstOrDefault(p => p.GetType() == GetType());
            if (finding != null)
            {
                //Deactivate it
                finding.OnDeactivated();
            }
        }
        tankController.ActivePowerUps.Add(this);
        TankData = tankData;
        Tank = tankController;
        Activated = true;
        //Hide the object
        Visible = false;
        if (!Forever)
        {
            StartCoroutine(Timer());
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Mathf.Clamp(LifeTime - WarningTime, 0f, LifeTime));
        OnWarning();
        yield return new WaitForSeconds(Mathf.Clamp(WarningTime, 0f, float.PositiveInfinity));
        OnDeactivated();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!Activated)
        {
            //If the collided object has a tank controller and tank data object on it
            var controller = other.GetComponent<Controller>();
            if (controller != null && controller.Data != null)
            {
                OnActivated(controller.Data, controller);
                //Activate the powerup
            }
        }
    }

    //Called when the powerup is about to run out
    public virtual void OnWarning()
    {

    }

    //Called when the powerup is deactivated
    //The base method removes the powerup from the list of powerups on the tank and destroys the powerup
    public virtual void OnDeactivated()
    {
        Tank.ActivePowerUps.Remove(this);
        Destroy(gameObject);
    }

}
