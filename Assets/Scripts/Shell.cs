﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class Shell : MonoBehaviour
{
    public float Lifetime { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }
    public Controller Source { get; set; }

    private Rigidbody body;

    //Sets the main parameters of the shell
    public void Set(float lifetime, float damage, float speed, Controller source)
    {
        Lifetime = lifetime; //Set the lifetime of the shell
        Damage = damage; //Set the damage the shell inflicts on an opposing tank
        Speed = speed; //Set the speed at which the shell travels
        Source = source; //Set the source tank that shot the shell
    }

    void Start()
    {
        //Get the rigidbody component
        body = GetComponent<Rigidbody>();
        //Make the shell move in the direction it is currently facing
        body.velocity = transform.forward * Speed;
        //Destroy the shell after a set lifetime
        Destroy(gameObject, Lifetime);
    }

    //When the shell has collided with anything
    private void OnTriggerEnter(Collider collision)
    {
        //If the shell's source has been destroyed, then there is no source to check against
        //OR if the source is still active, make sure that the shell is not accidentally colliding with it
        if (Source == null || collision.gameObject != Source.gameObject)
        {
            //If the shell has collided with something that registers for a shell hit, then call it
            var hitCallback = collision.gameObject.GetComponent<IOnShellHit>();
            if (hitCallback != null)
            {
                hitCallback.OnShellHit(this);
            }
            //Destroy the shell
            Destroy(gameObject);
        }
    }
}
