using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Tank Source; //The source tank
    float Timer; //The bomb countdown timer

    // Update is called once per frame
    void Update()
    {
        //Decrease the countdown timer
        Timer -= Time.deltaTime;
        //If the timer is up
        if (Timer <= 0)
        {
            Explosion.Spawn(transform.position, GameManager.Game.BombExplosionSize);
            //Spawn the explosion
            //var explosion = Instantiate(GameManager.Game.ExplosionPrefab, transform.position, Quaternion.identity);
            //Set the explosion's size
            //explosion.transform.localScale = Vector3.one * GameManager.Game.BombExplosionSize;
            //Destroy the explosion after 1 second
            //Destroy(explosion, 1f);
            //Damage all the tanks that are nearby
            for (int i = Tank.AllTanks.Count - 1; i >= 0; i--)
            {
                var tank = Tank.AllTanks[i];
                if (tank != Source && Vector3.Distance(tank.transform.position, transform.position) <= GameManager.Game.BombExplosionSize)
                {
                    tank.Attack(GameManager.Game.BombDamage);
                }
            }
            //Destroy this bomb object
            Destroy(gameObject);
        }
    }

    public static void Spawn(Tank source)
    {
        //Spawn the bomb
        var bomb = Instantiate(GameManager.Game.BombPrefab, source.transform.position, Quaternion.identity).GetComponent<Bomb>();
        //Set the stats
        bomb.Source = source;
        bomb.Timer = GameManager.Game.BombTime;
    }
}
