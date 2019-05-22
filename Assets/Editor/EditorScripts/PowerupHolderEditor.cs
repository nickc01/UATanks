using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(PowerUpInfo))]
public class PowerupInfoEditor : Editor
{
    SerializedProperty iter;
    PowerUpInfo holder;

    private void OnEnable()
    {
        iter = serializedObject.GetIterator();
        Debug.Log(iter.name);
        while (iter.NextVisible(true))
        {
            Debug.Log(iter.name);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        iter = serializedObject.GetIterator();
        iter.NextVisible(true);
        while (iter.NextVisible(false))
        {
            if (iter.name == nameof(holder.Script))
            {
                EditorGUILayout.LabelField("PowerUp",EditorStyles.boldLabel);
                EditorGUILayout.ObjectField(iter,typeof(MonoScript));
                EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.PropertyField(iter);
            }
        }


        serializedObject.ApplyModifiedProperties();
    }
}*/
