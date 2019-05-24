using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealthPowerup : PowerUp
{
    protected override void OnActivate()
    {
        //Increase the source tank's health
        Tank.Health += GameManager.Game.HealthRestoreAmount;
        //Destroy the powerup
        Destroy();
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void OnWarning()
    {
        
    }
}
