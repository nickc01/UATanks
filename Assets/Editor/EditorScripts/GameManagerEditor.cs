using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/*[CustomEditor(typeof(GameManager))]
[CanEditMultipleObjects]
public class GameManagerEditor : Editor
{
    SerializedProperty iter;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        iter = serializedObject.GetIterator();
        iter.NextVisible(true);
        while (iter.NextVisible(false))
        {
            RenderProperty(iter);
        }


        serializedObject.ApplyModifiedProperties();
    }

    private void RenderProperty(SerializedProperty prop)
    {
        EditorGUILayout.PropertyField(prop, true);
    }
}*/
