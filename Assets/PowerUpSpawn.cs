using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PowerupRoutine());
    }

    PowerUp SpawnedPowerUp = null;

    IEnumerator PowerupRoutine()
    {
        while (true)
        {
            /*var powerUpObject = GameManager.Game.PowerUpPrefabs.RandomElement();
            var powerUp = powerUpObject.GetComponent<PowerUp>();
            if (powerUp == null)
            {
                Debug.LogError("The power up of " + powerUpObject.name + " is not a valid powerUp. Make sure that it has a PowerUp Component attached attached to it.");
                continue;
            }
            yield return new WaitForSeconds(Random.Range(powerUp.SpawnTimeMinMax.x, powerUp.SpawnTimeMinMax.y));
            SpawnedPowerUp = Instantiate(powerUpObject, transform.position, transform.rotation).GetComponent<PowerUp>();
            yield return new WaitUntil(() => SpawnedPowerUp.Activated);
            SpawnedPowerUp = null;*/
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
