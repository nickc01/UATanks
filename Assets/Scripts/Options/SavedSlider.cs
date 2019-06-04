using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SavedSlider : MonoBehaviour
{
    [SerializeField] protected string SaveID;
    protected Slider slider; //The slider for the volume bar
    protected SavedValue<float> savedValue;
    private bool loaded = false;

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
        savedValue = new SavedValue<float>(SaveID);
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(newVal => 
        {
            OnSliderChange(Mathf.InverseLerp(slider.minValue,slider.maxValue,newVal));
        });
        Value = savedValue.Value;
    }

    protected virtual void OnSliderChange(float newValue)
    {
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
            return Mathf.InverseLerp(slider.minValue, slider.maxValue, slider.value);
        }
        set
        {
            if (!loaded)
            {
                Load();
            }
            savedValue.Value = value;
            slider.value = Mathf.Lerp(slider.minValue, slider.maxValue, value);
        }
    }
}
