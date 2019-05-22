using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;


public class PowerupHolder : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The type of powerup to use. Can be any script that inherits from the PowerUp Class")]
    public Object Script;

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

    private Type powerup;

    private void Start()
    {
        GetPowerupType();
    }

    private void GetPowerupType()
    {
        
    }
}
