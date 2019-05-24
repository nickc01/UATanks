using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public enum HoverMode
{
    Enter,
    Exit
}

public class ButtonHoverCallback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Action<HoverMode,PointerEventData> action = null;

    private void Start()
    {
        //Get the button name
        var methodName = gameObject.name + "Hover";
        //Remove spaces from the method name
        methodName = methodName.Replace(" ", "");
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
    }

    private static Action<HoverMode,PointerEventData> FindMethod(string name)
    {
        var method = typeof(Callbacks).GetMethod(name, BindingFlags.Static | BindingFlags.Public);
        if (method != null)
        {
            return (h,e) => method.Invoke(null,new object[2] { h,e });
        }
        return null;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        action(HoverMode.Enter,eventData);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        action(HoverMode.Exit,eventData);
    }
}
