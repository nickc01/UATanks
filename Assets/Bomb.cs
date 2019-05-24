using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Controller Source;
    float Timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0)
        {
            var explosion = Instantiate(GameManager.Game.ExplosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale = Vector3.one * GameManager.Game.BombExplosionSize;
            Destroy(explosion, 1f);
            for (int i = Controller.AllTanks.Count - 1; i >= 0; i--)
            {
                var tank = Controller.AllTanks[i];
                if (tank != Source && Vector3.Distance(tank.transform.position, explosion.transform.position) <= GameManager.Game.BombExplosionSize)
                {
                    tank.Attack(GameManager.Game.BombDamage);
                }
            }
            Destroy(gameObject);
        }
    }

    public static void Spawn(Controller source)
    {
        var bomb = Instantiate(GameManager.Game.BombPrefab, source.transform.position, Quaternion.identity).GetComponent<Bomb>();
        bomb.Source = source;
        bomb.Timer = GameManager.Game.BombTime;
    }
}
