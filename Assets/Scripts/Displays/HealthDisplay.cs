using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : Display<float>
{
    Slider healthSlider; //The slider that represents the health object

    public override float Value
    {
        get => base.Value;
        set
        {
            if (healthSlider == null)
            {
                healthSlider = GetComponent<Slider>();
            }
            base.Value = value;
            if (!Interpolate)
            {
                healthSlider.value = value;
            }
        }
    }

    protected override void Start()
    {
        healthSlider = GetComponent<Slider>();
    }

    public override void SetDirect(float value)
    {
        base.SetDirect(value);
        healthSlider.value = Value;
    }

    protected override void InterpolateTo(float newValue, float Speed)
    {
        //Interpolate to the current health value
        healthSlider.value = Mathf.Lerp(healthSlider.value, newValue, Speed * Time.deltaTime);
    }
}
