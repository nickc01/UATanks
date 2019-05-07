﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float Speed = 7f; //How fast the camera moves towards the target

    private static GameObject targetInternal; //The target the camera will move towards
    private static CameraController mainController; //The singleton for the camera controller

    public static GameObject Target
    {
        get => targetInternal;
        set => SetTarget(value);
    }

    private void Start()
    {
        //Set the singleton
        if (mainController == null)
        {
            mainController = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        if (targetInternal != null)
        {
            Vector3 targetPosition = targetInternal.transform.position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), Time.deltaTime * Speed);
        }
    }

    private static void SetTarget(GameObject target)
    {
        targetInternal = target;
        mainController.transform.position = new Vector3(target.transform.position.x,mainController.transform.position.y,target.transform.position.z);
    }
}
