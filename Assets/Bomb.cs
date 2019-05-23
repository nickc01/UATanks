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
            //TODO : BLOW UP
        }
    }

    public static void Spawn(Controller source)
    {
        var bomb = Instantiate(GameManager.Game.BombPrefab, source.transform.position, Quaternion.identity).GetComponent<Bomb>();
        bomb.Source = source;
        bomb.Timer = GameManager.Game.BombTime;
    }
}
