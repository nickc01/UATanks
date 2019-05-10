using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed = 7f; //How fast the camera moves towards the target

    private static GameObject targetInternal; //The target the camera will move towards
    public static CameraController Main { get; private set; } //The singleton for the camera controller

    public static GameObject Target
    {
        get => targetInternal;
        set => SetTarget(value);
    }

    public AudioSource Sound { get; private set; } //The Audio Source of the Camera

    private void Start()
    {
        //Set the singleton
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Set the Audio Source
        Sound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //If there is a target to move to
        if (targetInternal != null)
        {
            //Linearly interpolate to the target
            Vector3 targetPosition = targetInternal.transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), Time.deltaTime * Speed);
        }
    }

    //Sets the target that the camera should move to
    private static void SetTarget(GameObject target)
    {
        targetInternal = target;
        //Set the position of the camera to the target
        Main.transform.position = new Vector3(target.transform.position.x,Main.transform.position.y,target.transform.position.z);
    }
}
