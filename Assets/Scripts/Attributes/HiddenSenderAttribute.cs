using System;
using UnityEngine;

public class PropSenderAttribute : PropertyAttribute
{
    public object CompareValue; //The value to compare to
    public string BindID; //The bind ID
    public string PropertyName = null; //the name of the property to get the value from
    public bool NotEqual = false; //If true, will check for inequality

    public PropSenderAttribute(string bindID, bool compareValue = true)
    {
        BindID = bindID;
        CompareValue = compareValue;
    }

    public PropSenderAttribute(string valueID, string propertyName, object compareValue, bool notEqual = false)
    {
        if (compareValue is Enum)
        {
            compareValue = (int)compareValue;
        }
        BindID = valueID;
        CompareValue = compareValue;
        PropertyName = propertyName;
        NotEqual = notEqual;
    }
}