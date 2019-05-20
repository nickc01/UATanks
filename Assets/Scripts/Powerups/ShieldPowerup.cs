using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : PowerUp
{
    [Tooltip("How much damage the shield will resist")]
    [SerializeField] protected float ShieldStrength = 10f;
    [Tooltip("How fast the powerup will flicker when warning the player that it's about to run out. The value is how many flickers per second.")]
    [SerializeField] protected float WarningFlashRate = 20f;


    public override void OnActivated(TankData tankData, Controller tankController)
    {
        base.OnActivated(tankData, tankController);
        Visible = true;
        transform.parent = tankData.transform;
        transform.localPosition = Vector3.zero;
        tankData.DamageResistance += 10f;
    }

    public override void OnWarning()
    {
        StartCoroutine(WarningRoutine());
    }

    IEnumerator WarningRoutine()
    {
        while (true)
        {
            Visible = true;
            yield return new WaitForSeconds(1f / WarningFlashRate);
            Visible = false;
            yield return new WaitForSeconds(1f / WarningFlashRate);
        }
    }

    public override void OnDeactivated()
    {
        base.OnDeactivated();
        TankData.DamageResistance -= 10f;
    }
}
