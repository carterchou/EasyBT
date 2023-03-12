using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using EasyBT.Runtime;
using System.Reflection;

namespace EasyBT.Editor
{

    [CustomEditor(typeof(NodeInspector))]
    public class NodeInspectorEditor : UnityEditor.Editor
    {
        private Rect btnRect;
        private Rect popupbtnRect;
        public static NodeInspectorEditor instance { get; private set; }
        private NodeInspector nodeInspector;
        private Dictionary<string, Action<SerializedObject, NodeInspector>> drawFunction =
            new Dictionary<string, Action<SerializedObject, NodeInspector>>();

        private void OnDisable()
        {
            instance = null;
        }

        private void OnEnable()
        {
            instance = this;
            nodeInspector = (NodeInspector)target;

            var assembly = Assembly.Load("Assembly-CSharp-Editor");
            Type[] types = assembly.GetTypes();

            foreach (var item in types)
            {
                if (typeof(INodeInspectionDrawer).IsAssignableFrom(item) && item.Name != "INodeInspectionDrawer")
                {
                    var instance = assembly.CreateInstance("EasyBT.Editor." + item.Name);

                    drawFunction.Add(((NodeDrawer)instance.GetType().GetCustomAttribute(typeof(NodeDrawer))).nodeName,
                        (Action<SerializedObject, NodeInspector>)instance.GetType()
                        .GetMethod("Draw").CreateDelegate(typeof(Action<SerializedObject, NodeInspector>), instance));
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (NodeManager.instance.GetData()?.SelectedNodes.Count > 1)
            {
                GUILayout.Label("You only edit a node at same time");
            }
            else if (nodeInspector.currentNode != null)
            {
                if (nodeInspector.currentNode.parentNode != null
                    && nodeInspector.currentNode.parentNode.nodeName == "ProbabilityNode")
                {
                    //狦诀瞯竊翴竊翴Τ杠,ê竊翴磅︽诀瞯盢ッ环琌100%
                    if (nodeInspector.currentNode.parentNode.childNodes.Count == 1)
                    {
                        GUI.enabled = false;
                        nodeInspector.currentNode.probability = EditorGUILayout.Slider(
                        "Probability:", nodeInspector.currentNode.probability, 100f, 100f);
                        GUILayout.Space(5);
                        GUI.enabled = true;
                    }
                    else
                    {
                        //程计翴计
                        nodeInspector.currentNode.probability =
                            Mathf.RoundToInt(nodeInspector.currentNode.probability * 100f) * 0.01f;

                        nodeInspector.currentNode.probability = EditorGUILayout.Slider(
                            "Probability:", nodeInspector.currentNode.probability, 0, CalculationMaxProbability());
                        GUILayout.Space(5);
                    }
                }


                if (drawFunction.ContainsKey(nodeInspector.currentNode.nodeName))
                {
                    drawFunction[nodeInspector.currentNode.nodeName].Invoke(serializedObject, nodeInspector);
                }

                if (nodeInspector.currentNode.GetType().BaseType.Name == "CompositeNode")
                {
                    CompositeNode compositeNode = (CompositeNode)nodeInspector.currentNode;

                    GUI.enabled = true;
                    EditorGUILayout.PropertyField(serializedObject
                         .FindProperty("currentNode").FindPropertyRelative("description"), new GUIContent("Description:"), false);

                    EditorGUILayout.PropertyField(serializedObject
                         .FindProperty("currentNode").FindPropertyRelative("obstacleType"), new GUIContent("Abort Type:"), false);


                    if (compositeNode.obstacleType == CompositeNode.ObstacleType.None)
                    {

                        compositeNode.conditions = new List<ActWrap>();
                    }

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

                    if (compositeNode.obstacleType != CompositeNode.ObstacleType.None)
                    {
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
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private float CalculationMaxProbability()
        {
            var maxProbability = 100f;
            var currentNode = nodeInspector.currentNode;

            foreach (var subNode in currentNode.parentNode.childNodes)
            {
                if (subNode != nodeInspector.currentNode)
                {
                    maxProbability -= subNode.probability;
                }
            }

            return maxProbability;
        }
    }
}

