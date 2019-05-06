using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Shell Stats")]

    [Tooltip("How fast the tank fires a shell. Determines how many seconds are between each shot fired")]
    public float FireRate = 0.4f;
    [Tooltip("How fast each shell will travel when fired. In meters per second")]
    public float ShellSpeed = 4f;
    [Tooltip("How long the shell will live before it is automatically destroyed")]
    public float ShellLifetime = 10f;
    [Tooltip("How much damage the shell will inflict on an enemy")]
    public float ShellDamage = 25f;
}
