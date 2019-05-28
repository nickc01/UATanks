using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AudioSlider : MonoBehaviour
{
    protected abstract float VolumeSetter { get; set; }
    protected Slider slider;

    bool started = false;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!started)
        {
            started = true;
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(v =>
            {
                VolumeSetter = v;
            });
        }
    }

    protected void OnEnable()
    {
        if (!started)
        {
            Start();
        }
        slider.value = VolumeSetter;
    }
}

