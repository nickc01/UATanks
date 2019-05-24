using UnityEngine;

public class BombPowerup : PowerUp
{
    float BombTimer = 0f;

    protected override void OnActivate()
    {
        
    }

    protected override void OnDeactivate()
    {
        
    }

    protected override void Update()
    {
        BombTimer += Time.deltaTime;
        if (BombTimer >= GameManager.Game.BombRate)
        {
            BombTimer = 0f;
            Bomb.Spawn(Tank);
        }
    }

    protected override void OnWarning()
    {
        
    }
}

