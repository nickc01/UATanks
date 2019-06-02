public abstract class Display<ValueType> : PlayerSpecific
{
    private ValueType value = default; //The internal variable for storing the value of the display

    public virtual ValueType Value //The value of the display
    {
        get => value;
        set
        {
            this.value = value;
        }
    }

    protected virtual void Start() { }
}

