using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankMover), typeof(TankShooter), typeof(TankData))]
public abstract class Controller : MonoBehaviour, IOnShellHit
{
    protected TankMover Mover;
    protected TankShooter Shooter;
    protected TankData Data;

    public abstract void OnShellHit(Shell shell);

    public virtual void Start()
    {
        //Get the main tank components
        Mover = GetComponent<TankMover>();
        Shooter = GetComponent<TankShooter>();
        Data = GetComponent<TankData>();
    }
}
