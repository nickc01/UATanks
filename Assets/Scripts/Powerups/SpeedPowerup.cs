public class SpeedPowerup : PowerUp
{
    float OriginalForwardSpeed;
    float OriginalBackwardSpeed;

    protected override void OnActivate()
    {
        OriginalForwardSpeed = TankData.ForwardSpeed;
        OriginalBackwardSpeed = TankData.BackwardSpeed;
        TankData.ForwardSpeed = GameManager.Game.PowerupSpeed;
        TankData.BackwardSpeed = GameManager.Game.PowerupSpeed;
    }

    protected override void OnDeactivate()
    {
        TankData.ForwardSpeed = OriginalForwardSpeed;
        TankData.BackwardSpeed = OriginalBackwardSpeed;
    }

    protected override void OnWarning()
    {

    }
}
