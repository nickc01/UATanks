public class Invincibility : PowerUp
{
    public Invincibility(Tank Tank,float InvincibilityTime)
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
        Activate();
    }

    protected override void OnActivate()
    {
        
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void OnWarning()
    {
        
    }
}
