using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    [Tooltip("How fast the object should rotate to the new position")]
    [SerializeField] float Speed = 4f;
    Quaternion storage;

    private void Start()
    {
        storage = transform.rotation;
    }

    private void Update()
    {
        storage = Quaternion.Lerp(storage, transform.parent.rotation, Speed * Time.deltaTime);
        transform.rotation = storage;
    }
}
