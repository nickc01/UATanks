using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealthDisplay : MonoBehaviour
{
    private Slider healthSlider; //The slider that represents the health object
    private float healthInternal; //The internal variable for storing the health
    [SerializeField] bool Interpolate = true; //Whether to interpolate to the new health value or not
    [SerializeField] float InterpolationSpeed = 7f; //How fast to interpolate to the new health value
    [SerializeField] bool HideOnFull = true; //If true, hide the health bar when the health is full

    public float Health
    {
        get => healthInternal;
        set
        {
            //Clamps the health between 0 and 1
            healthInternal = Mathf.Clamp01(value);
            if (HideOnFull)
            {
                if (healthInternal == 1f)
                {
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    healthSlider.value = 1f;
                    transform.GetChild(0).gameObject.SetActive(true);
                }
            }
            if (!Interpolate)
            {
                //Set the slider to the health value
                healthSlider.value = healthInternal;
            }
        }
    }

    private void Start()
    {
        //Get the slider object and reset the health display
        healthSlider = GetComponent<Slider>();
        Health = 1.0f;
    }

    private void Update()
    {
        //If interpolation is enabled
        if (Interpolate)
        {
            //Interpolate to the new health value
            healthSlider.value = Mathf.Lerp(healthSlider.value, healthInternal, InterpolationSpeed * Time.deltaTime);
        }
    }
}
