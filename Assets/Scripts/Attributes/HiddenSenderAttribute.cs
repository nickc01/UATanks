using System;
using UnityEngine;

public class HiddenSenderAttribute : PropertyAttribute
{
    public object IfValueEqual;
    public string ValueID;
    public string PropertyName = null;
    public bool NotEqual = false;

    public HiddenSenderAttribute(string valueID, bool ifValueEqualTo = true)
    {
        ValueID = valueID;
        IfValueEqual = ifValueEqualTo;
    }

    public HiddenSenderAttribute(string valueID, string propertyName, object ifValueEqualTo, bool NotEqual = false)
    {
        if (ifValueEqualTo is Enum)
        {
            ifValueEqualTo = (int)ifValueEqualTo;
        }
        ValueID = valueID;
        IfValueEqual = ifValueEqualTo;
        PropertyName = propertyName;
        this.NotEqual = NotEqual;
    }
}