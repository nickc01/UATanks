using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : PowerUp
{
    protected float ShieldStrength;
    protected float WarningFlashRate;
    protected float DamageResistance;

    bool Warning = false;
    bool FlashOn = true;
    float Timer = 0f;
    Vector3 RotationVector = Vector3.up;

    protected override void OnActivate()
    {
        ShieldStrength = GameManager.Game.ShieldStrength;
        WarningFlashRate = GameManager.Game.ShieldWarningFlashRate;
        DamageResistance = GameManager.Game.ShieldDamageResistance;
        Holder.Visible = true;
        TankData.DamageResistance += 10f;
    }

    protected override void OnWarning()
    {
        Warning = true;
    }

    protected override void Update()
    {
        Holder.transform.Rotate(RotationVector * 180f * Mathf.Deg2Rad, Space.Self);
        if (Warning)
        {
            Timer += Time.deltaTime * WarningFlashRate;
            if (Timer >= 1f)
            {
                Timer = 0;
                FlashOn = !FlashOn;
                Holder.Visible = FlashOn;
            }
        }
    }

    protected override void OnDeactivate()
    {
        Holder.Visible = false;
        TankData.DamageResistance -= 10f;
    }
}
