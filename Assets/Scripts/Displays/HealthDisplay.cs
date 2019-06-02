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
}
