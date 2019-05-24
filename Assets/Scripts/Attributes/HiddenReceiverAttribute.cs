using UnityEngine;

public class PropReceiverAttribute : PropertyAttribute
{
    public string BindID; //The ID to bind to

    public PropReceiverAttribute(string bindID)
    {
        BindID = bindID;
    }
}