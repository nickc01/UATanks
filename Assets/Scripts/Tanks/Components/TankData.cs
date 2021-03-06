using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TankData : MonoBehaviour
{
    [Header("Tank Stats")]

    [Tooltip("How fast the tank moves forwards. In meters per second")]
    public float ForwardSpeed = 1f;
    [Tooltip("How fast the tank moves backwards. In meters per second")]
    public float BackwardSpeed = 1f;
    [Tooltip("How fast the tank rotates. In degrees per second")]
    public float RotateSpeed = 45f;
    [Tooltip("The starting health of the tank")]
    public float MaxHealth = 100f;
    [Tooltip("The current health of the tank. This value is set during gameplay")]
    [ReadOnly]
    public float Health = 0f;
    [ReadOnly]
    [Tooltip("The score of the current tank. This increases when it kills an opposing tank")]
    public float Score = 0f;
    [Tooltip("How many points this tank rewards the player when killed. This is primarily used on enemy tanks when they are killed and is ignored for player tanks")]
    public float TankValue = 25f;
    [Tooltip("The color of the tank")]
    [SerializeField] Color color = Color.white;
    [Tooltip("How much damage the tank will be able to resist per hit")]
    public float DamageResistance = 0f;
    [Tooltip("How many lives the tank has currently")]
    [ReadOnly]
    public int Lives = 1;
    [Tooltip("How many lives the tank will start out with")]
    public int MaxLives = 1;
    [Tooltip("How long the tank will take to respawn when it dies")]
    public float RespawnDelay = 1f;
    [Tooltip("How long the respawn invincibility will last")]
    public float RespawnInvincibility = 3f;
    [Tooltip("How fast the tank will flash when respawning. Number is in flashes per second")]
    public float RespawnFlashRate = 20f;

    [Header("Minimap")]
    [Tooltip("The prefab that will be used on the minimap to display where the tank is")]
    public MinimapPrefab MinimapObject;

    [Header("Shell Stats")]

    [Tooltip("How fast the tank fires a shell. Determines how many seconds are between each shot fired")]
    public float FireRate = 0.4f;
    [Tooltip("How fast each shell will travel when fired. In meters per second")]
    public float ShellSpeed = 4f;
    [Tooltip("How long the shell will live before it is automatically destroyed")]
    public float ShellLifetime = 10f;
    [Tooltip("How much damage the shell will inflict on an enemy")]
    public float ShellDamage = 25f;

    public Color TankColor
    {
        get => color;
        set
        {
            color = value;
            foreach (var colorizer in GetComponentsInChildren<TankColorer>())
            {
                colorizer.Color = value;
            }
        }
    }


    TankColorer[] colorizers;

    private void Start()
    {
        //Get all the colorizers on the current tank
        colorizers = GetComponentsInChildren<TankColorer>();
    }

    //BELOW IS USED TO DISPLAY THE CURRENT TANK COLOR IN THE EDITOR
    //THIS ALLOWS YOU TO ACTUALLY SEE THE COLOR YOU SET ON THE TANK BEFORE YOU HIT PLAY
#if UNITY_EDITOR
    private void Update()
    {
        //If the data is in edit mode
        if (!Application.IsPlaying(gameObject))
        {
            //Set the color of any colorizers on this object
            foreach (var colorizer in colorizers)
            {
                colorizer.Color = color;
            }
        }
    }

#endif

}
