using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AudioSlider : MonoBehaviour
{
    protected abstract float VolumeSetter { get; set; } //The property that is called when the slider changes
    protected Slider slider; //The slider for the volume bar

    bool started = false;
    protected virtual void Start()
    {
        if (!started)
        {
            started = true;
            //Add a callback to the volume slider that gets called when the slider changes
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(v =>
            {
                VolumeSetter = v;
            });
            slider.value = VolumeSetter;
        }
    }

    protected void OnEnable()
    {
        if (started)
        {
            //Refresh the slider when it becomes visible
            slider.value = VolumeSetter;
        }
    }
}

