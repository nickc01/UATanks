using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(HiddenReceiverAttribute))]
public class HiddenReceiver : PropertyDrawer
{
    HiddenReceiverAttribute parameters;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as HiddenReceiverAttribute;
        }
        if (HiddenSender.Enablers.ContainsKey(parameters.ValueID) && HiddenSender.Enablers[parameters.ValueID])
        {
            //EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property, label);
            //EditorGUI.EndProperty();
            //EditorGUI.PropertyField(position, property, label);
            //base.OnGUI(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as HiddenReceiverAttribute;
        }
        if (HiddenSender.Enablers != null && HiddenSender.Enablers.TryGetValue(parameters.ValueID, out var result) && result)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
            //return base.GetPropertyHeight(property,label);
        }
        return 0;
    }
}
