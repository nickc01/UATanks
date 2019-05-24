using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(PropReceiverAttribute))]
public class HiddenReceiver : PropertyDrawer
{
    PropReceiverAttribute parameters;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as PropReceiverAttribute;
        }
        if (HiddenSender.Enablers.ContainsKey(parameters.BindID) && HiddenSender.Enablers[parameters.BindID])
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as PropReceiverAttribute;
        }
        if (HiddenSender.Enablers != null && HiddenSender.Enablers.TryGetValue(parameters.BindID, out var result) && result)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        return 0;
    }
}
