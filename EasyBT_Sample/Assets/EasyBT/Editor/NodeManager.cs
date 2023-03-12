using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using System.Linq;
using EasyBT.Runtime;

namespace EasyBT.Editor
{
    public class NodeManager
    {
        private static NodeManager _nodeManager;
        public static NodeManager instance
        {
            get
            {
                if (_nodeManager == null)
                {
                    _nodeManager = new NodeManager();
                }

                return _nodeManager;
            }
        }

        public event Action onAddNewScript;
        private BehaviorTree behaviorTree;
        private NodeInspector nodeInspector;
        public List<Node> copiedBuffer = new List<Node>();

        private NodeManager() { }

        public void CreateNodeManager(BehaviorTree behaviorTree)
        {
            this.behaviorTree = behaviorTree;
            behaviorTree.behaviorData.tree = behaviorTree;
            this.nodeInspector = ScriptableObject.CreateInstance<NodeInspector>();
            this.nodeInspector.hideFlags = HideFlags.DontSaveInEditor;//避免再退出撥放模式遭到unity清除
        }

        public BehaviorData GetData()
        {
            if (this.behaviorTree != null)
            {
            
                return this.behaviorTree.behaviorData;
            }
            else
            {
                return null;
            }
        }

        public BehaviorTree GetTree()
        {
            return this.behaviorTree;
        }

        public void Save()
        {
            if (behaviorTree == null || behaviorTree.behaviorData == null)
                return;

            Node entryNode = behaviorTree.behaviorData.nodes.Find((node) => { return node.nodeName == "EntryNode"; });
            int index = behaviorTree.behaviorData.nodes.IndexOf(entryNode);
            Node tempNode = behaviorTree.behaviorData.nodes[0];
            behaviorTree.behaviorData.nodes[0] = entryNode;
            behaviorTree.behaviorData.nodes[index] = tempNode;

            OrderChildNodes();
            CancelSelected();

            foreach (var node in behaviorTree.behaviorData.nodes)
            {
                node.BreadthTranferObstacle();

                //確保機率節點一定會有個子節點被執行
                if (node.nodeName == "ProbabilityNode" && node.childNodes.Count > 0)
                {
                    float probabilities_sum = 0f;
                    float probabilities_sum_excludes_last = 0f;

                    for (int i = 0; i < node.childNodes.Count; i++)
                    {
                        probabilities_sum += node.childNodes[i].probability;

                        if (i != node.childNodes.Count - 1)
                        {
                            probabilities_sum_excludes_last += node.childNodes[i].probability;
                        }
                    }

                    if (probabilities_sum != 100f)
                    {
                        node.childNodes[node.childNodes.Count - 1].probability = 100f - probabilities_sum_excludes_last;
                    }
                }
            }

            behaviorTree.behaviorData.nodes[0].DepthTranferObstacle(new List<ActWrap>());

            EditorUtility.SetDirty(behaviorTree);
            Undo.ClearAll();
            behaviorTree = null;
        }

        public void MoveSelectedNodePos(Vector2 offset)
        {
            Undo.RegisterCompleteObjectUndo(behaviorTree, "Move");

            foreach (var item in behaviorTree.behaviorData.SelectedNodes)
            {
                if (item.nodeName == "EntryNode")
                    continue;
                item.rect.position += offset;
            }
        }

        private void ClearSelectedNods()
        {
            foreach (var node in behaviorTree.behaviorData.SelectedNodes)
            {
                node.priority = 0;
            }

            behaviorTree.behaviorData.SelectedNodes.Clear();
        }

        private void ClearSelectedLines()
        {
            foreach (var line in behaviorTree.behaviorData.connectLines)
            {
                line.color = Color.white;
            }

            behaviorTree.behaviorData.SelectedConnectLines.Clear();
        }

        /// <summary>
        ///  Set currently selected node then properties of node is displayed in the inspector
        /// </summary>
        /// <param name="nodes"></param>
        public void SelectNode(Node node, bool IsOverride)
        {
            if (IsOverride)
            {
                ClearSelectedLines();
                ClearSelectedNods();
                node.priority = 1;
                behaviorTree.behaviorData.SelectedNodes.Add(node);
            }
            else
            {
                node.priority = 1;
                behaviorTree.behaviorData.SelectedNodes.Add(node);
            }

            //Sort  priority of drawing
            behaviorTree.behaviorData.nodes.Sort();

            nodeInspector.currentNode = behaviorTree.behaviorData.SelectedNodes[0];
            Selection.activeObject = nodeInspector;
            if (NodeInspectorEditor.instance != null)
                NodeInspectorEditor.instance.Repaint();
        }

        public void SelectNodes(Rect rect)
        {
            ClearSelectedLines();
            ClearSelectedNods();
            List<Node> tempNodes = new List<Node>();

            foreach (var node in behaviorTree.behaviorData.nodes)
            {

                if (Mathc.AABB(rect, node.rect))
                {
                    tempNodes.Add(node);
                }
            }

            foreach (var tempNode in tempNodes)
            {
                behaviorTree.behaviorData.SelectedNodes.Add(tempNode);
            }

            Selection.activeObject = nodeInspector;
            if (NodeInspectorEditor.instance != null)
                NodeInspectorEditor.instance.Repaint();
        }

        public void SelectConnectLine(ConnectLine connectLine, bool IsOverride)
        {

            if (IsOverride)
            {
                ClearSelectedLines();
                ClearSelectedNods();
            }
            connectLine.color = new Color(0.3f, 0.7f, 1, 1f);
            behaviorTree.behaviorData.SelectedConnectLines.Add(connectLine);
        }

        public void CancelSelected()
        {
            ClearSelectedLines();
            ClearSelectedNods();

            nodeInspector.currentNode = null;
            if (NodeInspectorEditor.instance != null)
                NodeInspectorEditor.instance.Repaint();
        }

        private bool IsContain(Node node, List<Node> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (node.guid == nodes[i].guid)
                {
                    return true;
                }
            }

            return false;
        }

        public void JoinNode(Node node)
        {
            Undo.RegisterCompleteObjectUndo(behaviorTree, "CreateNode");

            while (IsContain(node, behaviorTree.behaviorData.nodes))
            {
                node.guid = Guid.NewGuid().ToString().Replace("-", "");
            }

            behaviorTree.behaviorData.nodes.Add(node);
        }

        public ConnectLine GetConnectLineOfMousePos(Vector2 mousePos)
        {
            foreach (var line in behaviorTree.behaviorData.connectLines)
            {
                var isHover = Mathc.IsPointHoverStraightLine
                    (line.outNode.rect.center, line.inNode.rect.center, mousePos, 10f);

                if (isHover)
                {

                    return line;
                }

            }
            return null;
        }


        public Node GetNodeOfMousePos(Vector2 mousePos)
        {

            for (int i = behaviorTree.behaviorData.nodes.Count - 1; i >= 0; i--)
            {
                if (behaviorTree.behaviorData.nodes[i].rect.Contains(mousePos))
                    return behaviorTree.behaviorData.nodes[i];
            }

            return null;
        }

        public List<Node> GetSelectedNodes()
        {
            return behaviorTree.behaviorData.SelectedNodes;
        }

        public void CreateNode<T>(Rect rect) where T : Node, new()
        {
            var t = new T();
            t.guid = Guid.NewGuid().ToString();
            t.nodeName = typeof(T).Name;
            t.rect = rect;
            JoinNode(t);
        }

        #region Static Method
        /// <summary>
        /// 使用字串創建新節點
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Node CreateNodeFromString(Rect rect, string className)
        {
            var assembly = Assembly.GetAssembly(typeof(BehaviorTree));
            var node = assembly.CreateInstance($"EasyBT.Runtime.{className}") as Node;

            if (node == null)
            {
                throw new Exception($"{className} does not exist");
            }

            node.guid = Guid.NewGuid().ToString();
            node.nodeName = className;
            node.rect = rect;

            return node;
        }


        /// <summary>
        /// Remove guid reference of nodes that are not in the tree
        /// </summary>
        public static void Trim(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                List<string> trimList = new List<string>();

                if (FindNodeWithGuid(node.parentNodeGuid, nodes) == null)
                    node.parentNodeGuid = null;


                foreach (var subNodeGuid in node.childNodeGuid)
                {
                    if (FindNodeWithGuid(subNodeGuid, nodes) == null)
                    {
                        trimList.Add(subNodeGuid);
                    }
                }

                foreach (var t in trimList)
                {
                    node.childNodeGuid.Remove(t);
                }

            }
        }

        private static Node FindNodeWithGuid(string guid, List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.guid == guid)
                {
                    return node;
                }
            }

            return null;
        }
        #endregion

        public void Copy()
        {
            if (behaviorTree.behaviorData.SelectedNodes.Count == 0)
                return;

            copiedBuffer.Clear();

            foreach (var selectedNode in behaviorTree.behaviorData.SelectedNodes)
            {
                Node copiedNode = new ActNode();

                if (selectedNode.nodeName == "EntryNode")
                    continue;
                else
                {
                    copiedNode = (Node)Assembly.GetAssembly(typeof(Node)).CreateInstance(selectedNode.GetType().FullName);
                }

                EditorUtility.CopySerializedManagedFieldsOnly(selectedNode, copiedNode);
                //Vector2 viewCoords = DesignerWindow.window.ConvertWorldToView(copiedNode.rect.position);
                //copiedNode.rect.position = viewCoords;
                copiedBuffer.Add(copiedNode);
            }

        }

        public void Paste()
        {
            List<Node> copiedList = new List<Node>();

            foreach (var selectedNode in copiedBuffer)
            {
                Node copiedNode = new ActNode();

                if (selectedNode.nodeName == "EntryNode")
                    continue;
                else
                {
                    copiedNode = (Node)Assembly.GetAssembly(typeof(Node)).CreateInstance(selectedNode.GetType().FullName);
                }

                EditorUtility.CopySerializedManagedFieldsOnly(selectedNode, copiedNode);
                copiedNode.rect.position += new Vector2(110, 0);
                //copiedNode.rect.position = DesignerWindow.window.ConvertViewToWorld(selectedNode.rect.position + new Vector2(110, 0));
                copiedList.Add(copiedNode);
            }

            Trim(copiedList);
            CreateReferenceWithGuid(copiedList);

            foreach (var copiedNode in copiedList)
            {

                while (IsContain(copiedNode, behaviorTree.behaviorData.nodes))
                {
                    copiedNode.guid = Guid.NewGuid().ToString();
                }
            }

            Trim(copiedList);
            ReferenceToGuid(copiedList);

            Undo.RegisterCompleteObjectUndo(behaviorTree, "Paste");

            //Start Paste
            foreach (var node in copiedList)
            {
                foreach (var subNode in node.childNodes)
                {
                    behaviorTree.behaviorData.connectLines.Add(new ConnectLine(node, subNode));
                }
            }

            ClearSelectedLines();
            ClearSelectedNods();

            //Paste node
            foreach (var copiedNode in copiedList)
            {
                behaviorTree.behaviorData.nodes.Add(copiedNode);
                behaviorTree.behaviorData.SelectedNodes.Add(copiedNode);
            }

            if (NodeInspectorEditor.instance != null)
                NodeInspectorEditor.instance.Repaint();
            else
                Selection.activeObject = nodeInspector;
        }

        private void CreateReferenceWithGuid(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                node.parentNode = FindNodeWithGuid(node.parentNodeGuid, nodes);
                node.childNodes.Clear();

                foreach (var subNodeGuid in node.childNodeGuid)
                {
                    var subNode = FindNodeWithGuid(subNodeGuid, nodes);
                    if (subNode != null)
                        node.childNodes.Add(subNode);
                }
            }
        }

        public void ReferenceToGuid(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (node.parentNode != null)
                    node.parentNodeGuid = node.parentNode.guid;

                for (int i = 0; i < node.childNodes.Count; i++)
                {
                    node.childNodeGuid.Add(node.childNodes[i].guid);
                }
            }
        }

        public void OrderChildNodes()
        {
            foreach (var node in behaviorTree.behaviorData.nodes)
            {
                if (node.childNodes.Count > 1)
                {
                    node.childNodeGuid.Clear();
                    var orderList = from subNode in node.childNodes
                                    orderby subNode.rect.x
                                    select subNode;

                    foreach (var item in orderList)
                    {
                        node.childNodeGuid.Add(item.guid);
                    }
                }
            }
        }

        private void RemoveLine(ConnectLine line)
        {
            line.outNode.childNodes.Remove(line.inNode);
            line.outNode.childNodeGuid.Remove(line.inNodeGuid);
            line.inNode.parentNode = null;
            line.inNode.parentNodeGuid = null;
            behaviorTree.behaviorData.connectLines.Remove(line);
        }

        public void DelectSelected()
        {

            if (behaviorTree.behaviorData.SelectedConnectLines.Count == 0 && behaviorTree.behaviorData.SelectedNodes.Count == 0)
                return;

            Undo.RegisterCompleteObjectUndo(behaviorTree, "Deleted");
            if (behaviorTree.behaviorData.SelectedConnectLines.Count > 0)
                DeleteSelectedConnectLines();
            else if (behaviorTree.behaviorData.SelectedNodes.Count > 0)
                DeleteSelectedNodes();
        }

        private void DeleteSelectedNodes()
        {
            foreach (var node in behaviorTree.behaviorData.SelectedNodes)
            {
                if (node.nodeName == "EntryNode")
                    continue;

                List<ConnectLine> deletedLines = new List<ConnectLine>();

                //找到和節點有關聯的連接線
                for (int i = 0; i < behaviorTree.behaviorData.connectLines.Count; i++)
                {
                    if (behaviorTree.behaviorData.connectLines[i].outNode == node || behaviorTree.behaviorData.connectLines[i].inNode == node)
                    {

                        deletedLines.Add(behaviorTree.behaviorData.connectLines[i]);
                    }
                }

                foreach (var item in deletedLines)
                {
                    RemoveLine(item);
                }

                deletedLines.Clear();
                behaviorTree.behaviorData.nodes.Remove(node);
            }

            behaviorTree.behaviorData.SelectedNodes.Clear();
        }

        private void DeleteSelectedConnectLines()
        {
            foreach (var line in behaviorTree.behaviorData.SelectedConnectLines)
            {
                RemoveLine(line);
            }

            ClearSelectedLines();
        }

        public void ConnectNode(Node inNode)
        {
            //葉子節點不能擁有子節點
            if (behaviorTree.behaviorData.SelectedNodes[0].nodeName == "ActNode"
                || behaviorTree.behaviorData.SelectedNodes[0].nodeName == "ConditionNode")
                return;

            var outNode = behaviorTree.behaviorData.SelectedNodes[0];

            if (inNode.nodeName == "EntryNode"
                || behaviorTree.behaviorData.SelectedNodes[0].Equals(inNode)
                || behaviorTree.behaviorData.SelectedNodes[0].childNodes.Contains(inNode))
            {
                return;
            }

            Undo.RegisterCompleteObjectUndo(behaviorTree, "ConnectNode");

            //如果被連接的節點原先有父節點，刪除該線
            if (inNode.parentNode != null)
            {
                var line = behaviorTree.behaviorData.FindConnectLineWithGuid(inNode.parentNode.guid, inNode.guid);
                if (line == null)
                    throw new Exception("Occurs when looking for a connection line");
                else
                    RemoveLine(line);
            }

            //除了組合節點以外的節點類型不能擁有多個子節點
            if (behaviorTree.behaviorData.SelectedNodes[0].childNodes.Count > 0 &&
                behaviorTree.behaviorData.SelectedNodes[0].GetType().BaseType.Name != "CompositeNode")
            {
                //找到對應的連接線並且刪除
                var parentNode = behaviorTree.behaviorData.SelectedNodes[0];
                var line = behaviorTree.behaviorData.FindConnectLineWithGuid(parentNode.guid, parentNode.childNodeGuid[0]);
                RemoveLine(line);
            }

            outNode.childNodes.Add(inNode);
            outNode.childNodeGuid.Add(inNode.guid);
            inNode.parentNode = outNode;
            inNode.parentNodeGuid = outNode.guid;

            ConnectLine connectLine = new ConnectLine(behaviorTree.behaviorData.SelectedNodes[0], inNode);
            behaviorTree.behaviorData.connectLines.Add(connectLine);
            return;
        }

        public void AddScriptToCurrentNode(MonoScript script)
        {
            Assembly assembly = Assembly.Load("Assembly-CSharp");
            object scriptInstance = assembly.CreateInstance(script.GetClass().FullName);

            Undo.RegisterCompleteObjectUndo(behaviorTree, "New Script");

            if (behaviorTree.behaviorData.SelectedNodes[0].GetType().Name == "ActNode")
            {
                Act act = (Act)scriptInstance;
                ActNode actNode = (ActNode)behaviorTree.behaviorData.SelectedNodes[0];
                actNode.actWrap = new ActWrap() { act = act, monoScript = script };
                onAddNewScript?.Invoke();
            }
            else if((behaviorTree.behaviorData.SelectedNodes[0].GetType().Name == "ConditionNode"))
            {
                Act act = (Act)scriptInstance;
                ((ConditionNode)behaviorTree.behaviorData.SelectedNodes[0]).conditions.Add(new ActWrap() { act = act, monoScript = script });
                onAddNewScript?.Invoke();
            }
            else
            {
                Act act = (Act)scriptInstance;
                ((CompositeNode)behaviorTree.behaviorData.SelectedNodes[0]).conditions.Add(new ActWrap() { act = act, monoScript = script });
                onAddNewScript?.Invoke();
            }
        }
    }
}

