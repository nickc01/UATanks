using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Locks the rotation of any object
public class RotationLock : MonoBehaviour
{
    Quaternion storage; //Stores the set local Rotation of the object

    private void Start()
    {
        //Set the starting rotation of the object
        storage = transform.localRotation;
    }
    private void LateUpdate()
    {
        //Lock the object's rotation to the local rotation set in storage
        transform.rotation = storage;
    }
}
