using UnityEngine;

public abstract class Display<ValueType> : PlayerSpecific
{
    [SerializeField] protected bool Interpolate = true; //Whether to interpolate to the new health value or not
    [SerializeField] protected float InterpolationSpeed = 7f; //How fast to interpolate to the new health value
    private ValueType value = default; //The internal variable for storing the value of the display

    public virtual ValueType Value //The value of the display
    {
        get => value;
        set
        {
            this.value = value;
        }
    }

    public virtual void SetDirect(ValueType value)
    {
        Value = value;
    }

    protected virtual void InterpolateTo(ValueType newValue,float Speed)
    {
        
    }

    protected virtual void Update()
    {
        if (Interpolate)
        {
            InterpolateTo(value, InterpolationSpeed);
        }
    }

    protected virtual void Start() { }
}

