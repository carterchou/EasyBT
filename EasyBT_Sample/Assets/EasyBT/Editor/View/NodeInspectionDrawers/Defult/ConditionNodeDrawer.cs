using EasyBT.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    [NodeDrawer(nodeName = "ConditionNode")]
    public class ConditionNodeDrawer : INodeInspectionDrawer
    {
        public Rect popupbtnRect;

        public void Draw(SerializedObject serializedObject, NodeInspector nodeInspector)
        {
            GUI.enabled = true;

            var compositeNode = ((ConditionNode)nodeInspector.currentNode);

            if (compositeNode.conditions.Count > 0)
            {
                GUILayout.Label("", GUI.skin.horizontalSlider);
                GUILayout.Space(15);

                GUILayout.BeginVertical();
                for (int i = 0; i < compositeNode.conditions.Count; i++)
                {

                    GUI.enabled = true;
                    EditorGUILayout.PropertyField(serializedObject
                        .FindProperty("currentNode").FindPropertyRelative("conditions").GetArrayElementAtIndex(i), new GUIContent("Condition:", NodeStyle.scriptTexture), false);

                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentNode").FindPropertyRelative("conditions").GetArrayElementAtIndex(i).
                        FindPropertyRelative("monoScript"), new GUIContent("MonoScript:"), false);

                    GUI.enabled = true;

                    if (compositeNode.conditions[i].monoScript != null && compositeNode.conditions[i].act == null)
                    {
                        EditorGUILayout.HelpBox("Missing types referenced", MessageType.Warning);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject
                            .FindProperty("currentNode").FindPropertyRelative("conditions").GetArrayElementAtIndex(i).FindPropertyRelative("act"), new GUIContent("Paremeters:"), true);
                    }

                    if (i != compositeNode.conditions.Count - 1)
                        GUILayout.Space(30);
                }
                GUILayout.EndVertical();

            }

            GUILayout.BeginHorizontal();
            GUILayout.Space((Screen.width - 260) / 2);

            if (GUILayout.Button("Add Condition", GUILayout.Width(250), GUILayout.Height(25)))
            {
                PopupWindow.Show(popupbtnRect,
                    new PopupWindowForAddScript(new Vector2(popupbtnRect.width, 250), PopupWindowForAddScript.FindType.Condition));
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
