using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class TankShooter : MonoBehaviour
{
    private Controller controller;
    public float FireRate { get; set; } = 0f; //The cooldown timer before another shell can be fired
    public bool Firing { get; private set; } = false; //Whether the tank is shooting a bullet or not
    private void Start()
    {
        controller = GetComponent<Controller>();
        StartCoroutine(CoolDownTracker());
    }
    public void Shoot(float Speed, float Damage, float Lifetime)
    {
        if (!Firing)
        {
            Firing = true;
            var newShell = Instantiate(Game.ShellPrefab, transform.position, transform.rotation).GetComponent<Shell>();
            newShell.Set(Lifetime, Damage, Speed, controller);
        }
    }

    //Keeps track of the cooldown between each shell fire
    IEnumerator CoolDownTracker()
    {
        while (true)
        {
            //Wait until the controller wants to fire a shell
            yield return new WaitUntil(() => Firing);
            yield return new WaitForSeconds(FireRate);
            Firing = false;
        }
    }
}
