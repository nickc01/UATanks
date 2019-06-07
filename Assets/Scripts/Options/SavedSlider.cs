using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SavedSlider : MonoBehaviour
{
    [SerializeField] protected float DefaultValue = 1.0f; //The default value for the slider
    [SerializeField] protected string SaveID; //The save ID for PlayerPrefs
    protected Slider slider; //The slider for the volume bar
    protected SavedValue<float> savedValue; //The saved value
    private bool loaded = false; //Whether this object is loaded or not

    protected void Start()
    {
        if (!loaded)
        {
            Load();
        }
    }

    protected virtual void Load()
    {
        loaded = true;
        if (SaveID == null || SaveID == "")
        {
            SaveID = GetType().FullName;
        }
        //Create the saved value object
        savedValue = new SavedValue<float>(SaveID,DefaultValue);
        //Get the slider component
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(newVal => 
        {
            OnSliderChange(Mathf.InverseLerp(slider.minValue,slider.maxValue,newVal));
        });
        Value = savedValue.Value;
    }

    //When the slider value has updated
    protected virtual void OnSliderChange(float newValue)
    {
        //Set the updated value
        Value = newValue;
    }

    public virtual float Value
    {
        get
        {
            if (!loaded)
            {
                Load();
            }
            //Get the slider value
            return Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        }
        set
        {
            if (!loaded)
            {
                Load();
            }
            //Set the slider value
            savedValue.Value = value;
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, value);
        }
    }
}
