using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : Display<float>
{
    Slider healthSlider; //The slider that represents the health object
    [SerializeField] bool Interpolate = true; //Whether to interpolate to the new health value or not
    [SerializeField] float InterpolationSpeed = 7f; //How fast to interpolate to the new health value

    public override float Value
    {
        get => base.Value;
        //set => healthSlider.value = base.Value = value;
        set
        {
            if (healthSlider == null)
            {
                healthSlider = GetComponent<Slider>();
            }
            healthSlider.value = base.Value = value;
        }
    }

    protected override void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        //If interpolation is enabled
        if (Interpolate)
        {
            //Interpolate to the current health value
            healthSlider.value = Mathf.Lerp(healthSlider.value, Value, InterpolationSpeed * Time.deltaTime);
        }
    }

    /*
    public int PlayerID { get; set; }

    public float Health
    {
        get => healthInternal;
        set
        {
            //Clamps the health between 0 and 1
            healthInternal = Mathf.Clamp01(value);
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
            //Interpolate to the current health value
            healthSlider.value = Mathf.Lerp(healthSlider.value, healthInternal, InterpolationSpeed * Time.deltaTime);
        }
    }*/
}
