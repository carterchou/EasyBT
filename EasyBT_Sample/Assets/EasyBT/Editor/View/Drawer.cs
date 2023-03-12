using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EasyBT.Runtime;

namespace EasyBT.Editor
{
    public class Drawer
    {
        public static void DrawNodes(List<Node> nodeList, Vector2 offset)
        {
            foreach (var node in nodeList)
            {
                var title = node.nodeName.Split("Node")[0];
                Rect rect = new Rect(new Vector2(node.rect.x - offset.x, node.rect.y - offset.y), node.rect.size);
                Rect helpRect = new Rect(rect.position, new Vector2(20, 20));

                switch (node.taskState)
                {
                    case TaskState.Failure:
                        GUI.Box(rect, title, NodeStyle.failureNodeStyle);
                        break;
                    case TaskState.Success:
                        GUI.Box(rect, title, NodeStyle.successNodeStyle);
                        break;
                    case TaskState.Running:
                        GUI.Box(rect, title, NodeStyle.runningNodeStyle);
                        break;
                    case TaskState.None:
                        GUI.Box(rect, title, NodeStyle.noneNodeStyle);
                        break;
                }

                //畫提醒標誌
                switch (node.nodeName)
                {
                    case "ActNode":

                        var actNode = (ActNode)node;

                        if (actNode.actWrap.monoScript == null)
                        {
                            GUI.Box(helpRect, "", NodeStyle.warningSign);
                        }
                        else if (actNode.actWrap.monoScript != null && actNode.actWrap.act == null)
                        {
                            GUI.Box(helpRect, "", NodeStyle.warningSign);
                        }
                        break;
                }

                //畫提醒標誌
                if (node.GetType().BaseType.Name == "CompositeNode")
                {
                    var compositeNode = (CompositeNode)node;

                    foreach (var item in compositeNode.conditions)
                    {
                        if (item.monoScript != null && item.act == null)
                        {
                            GUI.Box(helpRect, "", NodeStyle.warningSign);
                            break;
                        }
                    }
                }
            }
        }


        public static void DrawSelectedNodeFrame(List<Node> nodes, Vector2 offset)
        {
            foreach (var node in nodes)
            {
                Rect rect = new Rect(new Vector2(node.rect.x - offset.x, node.rect.y - offset.y), node.rect.size);
                GUI.Box(rect, "", NodeStyle.selectedNodeStyle);
            }
        }

        public static void DrawGrids(float spacing, Color color, Vector2 offset, float zoom)
        {
            Handles.color = color;

            //col
            for (int i = -500; i <= 500; i++)
            {
                Vector2 startPos = new Vector2(i * spacing - offset.x, 0);

                if ((GUI.matrix * startPos).x <= 0 || (GUI.matrix * startPos).x >= Screen.width)
                {
                    continue;
                }

                Vector2 endPos = new Vector2(i * spacing - offset.x, Screen.height * 1 / zoom);
                Handles.DrawLine(startPos, endPos);
            }

            //row
            for (int i = -500; i <= 500; i++)
            {
                Vector2 startPos = new Vector2(0, i * spacing - offset.y);

                if ((GUI.matrix * startPos).y <= 0 || (GUI.matrix * startPos).y >= Screen.height)
                {
                    continue;
                }

                Vector2 endPos = new Vector2(Screen.width * 1 / zoom, i * spacing - offset.y);
                Handles.DrawLine(startPos, endPos);
            }

            Handles.color = Color.white;
        }

        public static void DrawBackGround(float windowWidth, float windowHeight, Texture backGroundTex)
        {
            GUI.DrawTexture(new Rect(0, 0, windowWidth, windowHeight), backGroundTex);
        }

        public static void DrawUnselectedGameObjectView(float windowWidth, float windowHeight)
        {
            GUILayout.BeginArea(new Rect(0, 0, windowWidth, windowHeight));
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 16 };
            GUILayout.Label("To start EasyBT, select a GameObject", style, GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
        }

        public static void DrawSelectedIsPrefabAssetView(float windowWidth, float windowHeight)
        {
            GUILayout.BeginArea(new Rect(0, 0, windowWidth, windowHeight));
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 16 };
            GUILayout.Label("Due to serialization issues, you cannot " +
                "edit directly on the prefabAssest, please open the prefabAssest for editing", style, GUILayout.ExpandHeight(true));
            GUILayout.EndArea();
        }

        public static bool DrawCreateBehaviorTreeView(float windowWidth, float windowHeight)
        {
            var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.LowerCenter, fontSize = 16 };

            GUILayout.Label($"To Start using EasyBT with {Selection.activeGameObject.name}" +
                $",you must have BehaviorTree and BehaviorData asset"
                , style, GUILayout.Width(windowWidth), GUILayout.Height(windowHeight / 2));

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Create", GUILayout.Width(100)))
            {
                GUILayout.EndHorizontal();
                return true;
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            return false;
        }

        public static void DrawSelectFrame(Rect rect)
        {
            if (Mathf.Abs(rect.height) < 2 || Mathf.Abs(rect.width) < 2)
                return;

            Vector2 leftTop = rect.center + new Vector2(-Mathf.Abs(rect.size.x / 2), -Mathf.Abs(rect.size.y / 2));
            Vector2 rightTop = rect.center + new Vector2(Mathf.Abs(rect.size.x / 2), -Mathf.Abs(rect.size.y / 2));
            Vector2 leftBottom = rect.center + new Vector2(-Mathf.Abs(rect.size.x / 2), Mathf.Abs(rect.size.y / 2));
            Vector2 rightBottom = rect.center + new Vector2(Mathf.Abs(rect.size.x / 2), Mathf.Abs(rect.size.y / 2));

            DrawConnectLine(leftTop, rightTop, Color.white, 3);
            DrawConnectLine(leftTop, leftBottom, Color.white, 3);
            DrawConnectLine(leftBottom, rightBottom, Color.white, 3);
            DrawConnectLine(rightBottom, rightTop, Color.white, 3);

            GUI.Box(rect, "", NodeStyle.selectFrame);
        }

        public static void DrawConnectLine(Vector3 startPos, Vector3 endPos, Color color, float width)
        {
            var lerpStartPos = Vector3.Lerp(startPos, endPos, 0.1f);
            var lerpEndPos = Vector3.Lerp(startPos, endPos, 0.9f);
            Handles.DrawBezier(startPos, endPos, lerpStartPos, lerpEndPos, color, null, width);
        }

        public static void DrawConnectLines(List<ConnectLine> connectLines, Vector2 offset)
        {
            foreach (var line in connectLines)
            {
                Vector2 startPos = line.inNode.rect.center - offset;
                Vector2 endPos = line.outNode.rect.center - offset;
                DrawConnectLine(startPos, endPos, line.color, 3.5f);
            }
        }
    }
}

