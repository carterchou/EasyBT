using EasyBT.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    [NodeDrawer(nodeName = "ActNode")]
    public class ActionNodeDrawer : INodeInspectionDrawer
    {
        public Rect popupbtnRect;

        public void Draw(SerializedObject serializedObject, NodeInspector nodeInspector)
        {
            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode").FindPropertyRelative("actWrap")
                .FindPropertyRelative("monoScript"), new GUIContent("MonoScript:"), false);
            GUI.enabled = true;

            ActNode actNode = ((ActNode)nodeInspector.currentNode);

            if (actNode.actWrap.monoScript == null)
            {
                EditorGUILayout.HelpBox("Must have a script", MessageType.Warning);
            }
            else if (actNode.actWrap.monoScript != null && actNode.actWrap.act == null)
            {
                EditorGUILayout.HelpBox("Missing types referenced", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode").FindPropertyRelative("actWrap").FindPropertyRelative("act")
                    , new GUIContent("Paremeters"), true);
            }

            GUILayout.Label("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Space((Screen.width - 260) / 2);

            if (GUILayout.Button("Add Script", GUILayout.Width(250), GUILayout.Height(25)))
            {
                PopupWindow.Show(popupbtnRect,
                    new PopupWindowForAddScript(new Vector2(popupbtnRect.width, 250), PopupWindowForAddScript.FindType.Act));
            }

            if (Event.current.type == EventType.Repaint)
                popupbtnRect = GUILayoutUtility.GetLastRect();
            GUILayout.EndHorizontal();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode").
                FindPropertyRelative("obstacleCondition"), new GUIContent("Abort Condition:"));
            GUI.enabled = true;
        }
    }
}
