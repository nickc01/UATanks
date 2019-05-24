public class SpeedPowerup : PowerUp
{
    float OriginalForwardSpeed;
    float OriginalBackwardSpeed;

    protected override void OnActivate()
    {
        //Set the stats
        OriginalForwardSpeed = TankData.ForwardSpeed;
        OriginalBackwardSpeed = TankData.BackwardSpeed;
        //Increase the speed
        TankData.ForwardSpeed = GameManager.Game.PowerupSpeed;
        TankData.BackwardSpeed = GameManager.Game.PowerupSpeed;
    }

    protected override void OnDeactivate()
    {
        //Reset the speed
        TankData.ForwardSpeed = OriginalForwardSpeed;
        TankData.BackwardSpeed = OriginalBackwardSpeed;
    }

    protected override void OnWarning() { }
}

