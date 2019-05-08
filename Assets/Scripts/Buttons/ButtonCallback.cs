using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine.Events;

//Uses reflection to find the corresponding callback function within the "Callbacks.cs" class
public class ButtonCallback : MonoBehaviour
{
    UnityAction action = null;

    private void Start()
    {
        //Get the button name
        var methodName = gameObject.name;
        //Remove spaces from the method name
        methodName = methodName.Replace(" ","");
        //Find the callback method
        action = FindMethod(methodName);
        //If it has not been found yet
        if (action == null)
        {
            //Remove the string "Button" or "button" from the string
            methodName = methodName.Replace("Button", "");
            methodName = methodName.Replace("button", "");
            action = FindMethod(methodName);
            //If it is still not found
            if (action == null)
            {
                //Throw an exception
                throw new Exception("Cannot find callback for " + gameObject.name);
            }
        }
        //Call the callback method when the button is clicked
        GetComponent<Button>().onClick.AddListener(action);
    }

    private static UnityAction FindMethod(string name)
    {
        var method = typeof(Callbacks).GetMethod(name, BindingFlags.Static | BindingFlags.Public);
        if (method != null)
        {
            return () => method.Invoke(null,null);
        }
        return null;
    }
}
