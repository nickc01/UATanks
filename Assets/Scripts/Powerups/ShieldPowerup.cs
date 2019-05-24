using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : PowerUp
{
    protected float ShieldStrength;
    protected float WarningFlashRate;
    protected float DamageResistance;

    bool Warning = false; //Whether the warning is active or not
    bool FlashOn = true; //Wether to flash on or off
    float FlashTimer = 0f; //The flash timer
    Vector3 RotationVector = Vector3.up; //The direction to rotate

    protected override void OnActivate()
    {
        //Set the stats
        ShieldStrength = GameManager.Game.ShieldStrength;
        WarningFlashRate = GameManager.Game.ShieldWarningFlashRate;
        DamageResistance = GameManager.Game.ShieldDamageResistance;
        //Make the holder visible
        Holder.Visible = true;
        //Increase the tank's damage resistance
        TankData.DamageResistance += DamageResistance;
    }

    protected override void OnWarning()
    {
        //Enable the warning
        Warning = true;
    }

    protected override void Update()
    {
        //Rotate the holder
        Holder.transform.Rotate(RotationVector * 180f * Mathf.Deg2Rad, Space.Self);
        //If the warning is on
        if (Warning)
        {
            //Increase the flash timer
            FlashTimer += Time.deltaTime * WarningFlashRate;
            //If it's greater than 1
            if (FlashTimer >= 1f)
            {
                //Change the flash mode
                FlashTimer = 0;
                FlashOn = !FlashOn;
                Holder.Visible = FlashOn;
            }
        }
    }

    protected override void OnDeactivate()
    {
        //Hide the holder
        Holder.Visible = false;
        //Reset the damage resistance
        TankData.DamageResistance -= DamageResistance;
    }
}
