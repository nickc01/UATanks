using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Represents a powerup spawnpoint
public class PowerUpSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Start the powerup routine
        StartCoroutine(PowerupRoutine());
    }

    PowerupHolder SpawnedPowerUp = null; //The powerup that has spawned at this point

    IEnumerator PowerupRoutine()
    {
       while (true)
       {
            //Get a random powerup to spawn
            var powerUp = GameManager.Game.PowerUps.RandomElement();
            if (powerUp == null)
            {
                Debug.LogError("The power up of " + powerUp.name + " is not a valid powerUp. Make sure that it has a PowerUp Component attached attached to it.");
                continue;
            }
            //Wait a random amount of time
            yield return new WaitForSeconds(Random.Range(powerUp.powerUp.SpawnTimeMinMax.x, powerUp.powerUp.SpawnTimeMinMax.y));
            //Spawn the powerup
            SpawnedPowerUp = Instantiate(powerUp.gameObject, transform.position, powerUp.gameObject.transform.rotation).GetComponent<PowerupHolder>();
            //Wait untill it has been collected
            yield return new WaitUntil(() => SpawnedPowerUp.Activated);
            SpawnedPowerUp = null;
        }
        
    }
}
