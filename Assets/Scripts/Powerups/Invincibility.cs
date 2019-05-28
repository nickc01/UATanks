using UnityEngine;

public class Invincibility : PowerUp
{
    float FlashTimer = 0f;
    bool FlashTracker = true;

    float flashRate;

    float OldResistance = 0f;

    public Invincibility(Tank Tank,float InvincibilityTime,float FlashRate)
    {
        Info = new PowerUpInfo
        {
            LifeTime = InvincibilityTime,
            WarningTime = InvincibilityTime,
            Forever = false,
            OneAtATime = true
        };
        this.Tank = Tank;
        TankData = Tank.Data;
        Holder = null;
        flashRate = FlashRate;
        Activate();
    }

    protected override void OnActivate()
    {
        OldResistance = TankData.DamageResistance;
        TankData.DamageResistance = float.PositiveInfinity;
    }

    protected override void Update()
    {
        if ((FlashTimer += Time.deltaTime) >= 1f / flashRate)
        {
            Tank.Visible = FlashTracker = !FlashTracker;
        }
    }

    protected override void OnDeactivate()
    {
        Tank.Visible = true;
        TankData.DamageResistance = OldResistance;
    }

    protected override void OnWarning() { }
}
