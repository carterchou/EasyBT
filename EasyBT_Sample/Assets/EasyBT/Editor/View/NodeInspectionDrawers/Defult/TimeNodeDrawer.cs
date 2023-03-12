using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    [NodeDrawer(nodeName = "TimeNode")]
    public class TimeNodeDrawer : INodeInspectionDrawer
    {
        public void Draw(SerializedObject serializedObject, NodeInspector nodeInspector)
        {
            GUI.enabled = true;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode")
                .FindPropertyRelative("coolDownTime"), new GUIContent("Time:"), false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode")
               .FindPropertyRelative("executedFirstTime"), new GUIContent("ExecutedFirstTime:"), false);
        }
    }
}
