using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(ReferenceLimitAttribute))]
public class ReferenceLimit : PropertyDrawer
{
    ReferenceLimitAttribute parameters;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as ReferenceLimitAttribute;
        }
        EditorGUI.ObjectField(position, property, parameters.limitType);
    }
}
