using UnityEngine;

public class Invincibility : PowerUp
{
    float FlashTimer = 0f; //Keeps track of the flash
    bool FlashTracker = true; //Keeps track of whether to flash on or off

    float flashRate; //How fast the tank should flash

    float OldResistance = 0f; //The original resistance of the tank

    public Invincibility(Tank Tank,float InvincibilityTime,float FlashRate)
    {
        //Setup the stats
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
        //Activate the powerup
        Activate();
    }

    protected override void OnActivate()
    {
        //Replace the old resistance with infinite resistance
        OldResistance = TankData.DamageResistance;
        TankData.DamageResistance = float.PositiveInfinity;
    }

    protected override void Update()
    {
        //Cause the tank to flash
        if ((FlashTimer += GameManager.GameDT) >= 1f / flashRate)
        {
            Tank.Visible = FlashTracker = !FlashTracker;
        }
    }

    protected override void OnDeactivate()
    {
        //Set the tank to it's original resistance
        Tank.Visible = true;
        TankData.DamageResistance = OldResistance;
    }

    protected override void OnWarning() { }
}
