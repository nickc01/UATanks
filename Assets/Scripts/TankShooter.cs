using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class TankShooter : MonoBehaviour
{
    private Controller controller; //The tank controller of this object
    public float FireRate { get; set; } = 0f; //The cooldown timer before another shell can be fired
    public bool Firing { get; private set; } = false; //Whether the tank is shooting a bullet or not
    private void Start()
    {
        //Get the tank controller
        controller = GetComponent<Controller>();
        //Start the cooldown tracker
        StartCoroutine(CoolDownTracker());
    }
    //Shoots a shell with a set speed, damage, and lifetime
    public void Shoot(float Speed, float Damage, float Lifetime)
    {
        //If the firing cooldown is still present, then do not fire a shell
        if (!Firing)
        {
            //Otherwise, create a new shell object and set its stats
            Firing = true;
            var newShell = Instantiate(Game.ShellPrefab, transform.position, transform.rotation).GetComponent<Shell>();
            newShell.Set(Lifetime, Damage, Speed, controller,1f);
        }
    }

    //Keeps track of the cooldown between each shell fire
    IEnumerator CoolDownTracker()
    {
        while (true)
        {
            //Wait until the controller wants to fire a shell
            yield return new WaitUntil(() => Firing);
            //Wait the set firing cooldown
            yield return new WaitForSeconds(FireRate);
            //Ready to fire another shell
            Firing = false;
        }
    }
}
