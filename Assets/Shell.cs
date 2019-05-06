using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class Shell : MonoBehaviour
{
    public float Lifetime { get; private set; }
    public float Damage { get; private set; }
    public float Speed { get; private set; }
    public Controller Source { get; private set; }

    private Rigidbody body;

    //Sets the main parameters of the shell
    public void Set(float lifetime, float damage, float speed, Controller source)
    {
        Lifetime = lifetime;
        Damage = damage;
        Speed = speed;
        Source = source;
    }

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.velocity = transform.forward * Speed;
        Destroy(gameObject, Lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != Source.gameObject)
        {
            var hitCallback = collision.gameObject.GetComponent<IOnShellHit>();
            if (hitCallback != null)
            {
                hitCallback.OnShellHit(this);
            }
            Destroy(gameObject);
        }
    }
}
