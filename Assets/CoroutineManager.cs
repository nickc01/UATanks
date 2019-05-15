using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public new static Coroutine StartCoroutine(IEnumerator Routine)
    {
        return Singleton.StartCoroutine(Routine);
    }
    public new static void StopCoroutine(Coroutine Routine)
    {
        Singleton.StopCoroutine(Routine);
    }
}
