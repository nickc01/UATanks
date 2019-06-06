using UnityEngine;

public class BombPowerup : PowerUp
{
    float BombTimer = 0f;

    protected override void OnActivate() { }

    protected override void OnDeactivate() { }

    protected override void Update()
    {
        //Increase the bomb timer
        BombTimer += GameManager.GameDT;
        //If the bomb timer is greater than the bomb rate
        if (BombTimer >= GameManager.Game.BombRate)
        {
            //Reset the timer
            BombTimer = 0f;
            //Spawn a bomb at the source tank's position
            Bomb.Spawn(Tank);
        }
    }

    protected override void OnWarning() { }
}

