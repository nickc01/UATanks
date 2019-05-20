using UnityEngine;

public class HiddenReceiverAttribute : PropertyAttribute
{
    public string ValueID;

    public HiddenReceiverAttribute(string valueID)
    {
        ValueID = valueID;
    }
}