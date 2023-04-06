using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    [NodeDrawer(nodeName = "FixedCountNode")]
    public class FixedCountNodDrawer : INodeInspectionDrawer
    {
        public void Draw(SerializedObject serializedObject, NodeInspector nodeInspector)
        {
			GUI.enabled = true;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode")
				.FindPropertyRelative("count"), new GUIContent("count:"), false);
        }
    }
}
