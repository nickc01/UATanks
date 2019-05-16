using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gives access to starting and stopping coroutines in static methods
public class CoroutineManager : MonoBehaviour
{
    static MonoBehaviour Singleton; //The singleton for the coroutine manager
    void Start()
    {
        //Set the singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Starts a coroutine
    public new static Coroutine StartCoroutine(IEnumerator Routine)
    {
        return Singleton.StartCoroutine(Routine);
    }
    //Stops a coroutine
    public new static void StopCoroutine(Coroutine Routine)
    {
        Singleton.StopCoroutine(Routine);
    }
}
