using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private static HealthDisplay Singleton; //The main singleton of the health display
    private Slider healthSlider; //The slider that represents the health object
    private float healthInternal; //The internal variable for storing the health
    [SerializeField] bool Interpolate = true; //Whether to interpolate to the new health value or not
    [SerializeField] float InterpolationSpeed = 7f; //How fast to interpolate to the new health value

    public static float Health
    {
        get => Singleton.healthInternal;
        set
        {
            //Clamps the health between 0 and 1
            Singleton.healthInternal = Mathf.Clamp01(value);
            if (!Singleton.Interpolate)
            {
                //Set the slider to the health value
                Singleton.healthSlider.value = Singleton.healthInternal;
            }
        }
    }

    private void Start()
    {
        //Set the singleton
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        //Get the slider object and reset the health display
        healthSlider = GetComponent<Slider>();
        Health = 1.0f;
    }

    private void Update()
    {
        if (Interpolate)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, healthInternal, InterpolationSpeed * Time.deltaTime);
        }
    }
}
