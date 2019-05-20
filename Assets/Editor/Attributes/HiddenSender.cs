using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(HiddenSenderAttribute))]
public class HiddenSender : PropertyDrawer
{
    public static Dictionary<string, bool> Enablers = new Dictionary<string, bool>();

    HiddenSenderAttribute parameters;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (parameters == null)
        {
            parameters = attribute as HiddenSenderAttribute;
        }
        EditorGUI.PropertyField(position, property, label);

        bool result = false;
        if (parameters.PropertyName != null)
        {
            result = GetValueFromProperty(property.serializedObject.FindProperty(parameters.PropertyName)).Equals(parameters.IfValueEqual);
        }
        else
        {
            result = parameters.IfValueEqual.Equals(property.boolValue);
        }
        if (parameters.NotEqual)
        {
            result = !result;
        }
        if (!Enablers.ContainsKey(parameters.ValueID))
        {
            Enablers.Add(parameters.ValueID, result);
        }
        else
        {
            Enablers[parameters.ValueID] = result;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    private static object GetValueFromProperty(SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
                return null;
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.Color:
                return property.colorValue;
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue;
            case SerializedPropertyType.LayerMask:
                return null;
            case SerializedPropertyType.Enum:
                return property.enumValueIndex;
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            case SerializedPropertyType.Vector3:
                return property.vector3Value;
            case SerializedPropertyType.Vector4:
                return property.vector4Value;
            case SerializedPropertyType.Rect:
                return property.rectValue;
            case SerializedPropertyType.ArraySize:
                return property.arraySize;
            case SerializedPropertyType.Character:
                return null;
            case SerializedPropertyType.AnimationCurve:
                return property.animationCurveValue;
            case SerializedPropertyType.Bounds:
                return property.boundsValue;
            case SerializedPropertyType.Gradient:
                return null;
            case SerializedPropertyType.Quaternion:
                return property.quaternionValue;
            case SerializedPropertyType.ExposedReference:
                return property.exposedReferenceValue;
            case SerializedPropertyType.FixedBufferSize:
                return property.fixedBufferSize;
            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue;
            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue;
            case SerializedPropertyType.RectInt:
                return property.rectIntValue;
            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue;
            default:
                return null;
        }
    }
}
