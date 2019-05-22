using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;


[Serializable]
public class PowerUpInfo
{
    [SerializeField]
    [Tooltip("The name of the powerup")]
    private string Name;
    [Tooltip("The type of powerup to use. Can be any script that inherits from the PowerUp Class")]
#if UNITY_EDITOR
    [ReferenceLimit(typeof(MonoScript))]
#endif
    [SerializeField]
    UnityEngine.Object Script;

    [Header("Powerup Stats")]
    [Tooltip("Whether the powerup will last forever or not")]
    [HiddenSender("Forever", false)]
    public bool Forever = false;
    [Tooltip("How long the powerup will last")]
    [HiddenReceiver("Forever")]
    public float LifeTime = 9f;
    [HiddenReceiver("Forever")]
    [Tooltip("The warning time used to warn the player when the powerup is about to run out")]
    public float WarningTime = 3f;
    [Tooltip("How long it takes for the powerup to spawn in the game")]
    public Vector2 SpawnTimeMinMax;
    [Tooltip("If true, will make sure that tanks cannot have more than one of this powerup active on them at once")]
    public bool OneAtATime = false;
    

    public Type PowerUpType
    {
        get
        {
            //Convert the script name into a type
            var type = Assembly.GetExecutingAssembly().GetType(Script.name);
            if (type == null)
            {
                Debug.LogError("Script of " + Script.name + " could not be found");
            }
            if (type.IsSubclassOf(typeof(PowerUp)) && !type.IsAbstract)
            {
                return type;
            }
            else
            {
                Debug.LogError("Type of " + type.Name + " does not inherit from " + typeof(PowerUp).Name);
            }
            return null;
        }
    }

}
