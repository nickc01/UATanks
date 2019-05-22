using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealthPowerup : PowerUp
{
    protected override void OnActivate()
    {
        Tank.Health += GameManager.Game.HealthRestoreAmount;
        Destroy();
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void OnWarning()
    {
        
    }
}
