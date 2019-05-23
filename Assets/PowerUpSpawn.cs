using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents a powerup spawnpoint
public class PowerUpSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PowerupRoutine());
    }

    PowerupHolder SpawnedPowerUp = null;

    IEnumerator PowerupRoutine()
    {
        while (true)
       {
            var powerUp = GameManager.Game.PowerUps.RandomElement();
            if (powerUp == null)
            {
                Debug.LogError("The power up of " + powerUp.name + " is not a valid powerUp. Make sure that it has a PowerUp Component attached attached to it.");
                continue;
            }
            yield return new WaitForSeconds(Random.Range(powerUp.powerUp.SpawnTimeMinMax.x, powerUp.powerUp.SpawnTimeMinMax.y));
            SpawnedPowerUp = Instantiate(powerUp.gameObject, transform.position, powerUp.gameObject.transform.rotation).GetComponent<PowerupHolder>();
            yield return new WaitUntil(() => SpawnedPowerUp.Activated);
            SpawnedPowerUp = null;
        }
        
    }
}
