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
            GameObject.Instantiate(GameManager.Game.BombPrefab, Tank.transform.position, Quaternion.identity);
        }
    }

    protected override void OnWarning()
    {
        
    }
}

