using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;

namespace EasyBT.Runtime
{
    [Serializable]
    public class BehaviorData : ISerializationCallbackReceiver
    {
        [SerializeReference]
        public List<Node> nodes = new List<Node>();
        public BehaviorTree tree;

#if UNITY_EDITOR
        [HideInInspector]
        public List<ConnectLine> SelectedConnectLines = new List<ConnectLine>();
        [SerializeReference]
        public List<Node> SelectedNodes = new List<Node>();
        public List<ConnectLine> connectLines = new List<ConnectLine>();
#endif

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR

            List<Node> m_nodes = new List<Node>();

            //清除那些序列化失敗的節點
            foreach (var node in nodes)
            {
                if (node == null)
                {
                    m_nodes.Add(node);
                }
            }

            foreach (var node in m_nodes)
            {
                nodes.Remove(node);
            }

#endif
            InitTree();
        }

        private Node FindNodeWithGuid(string id)
        {
            foreach (var node in nodes)
            {
                if (node.guid == id)
                {
                    return node;
                }
            }

            return null;
        }

        private void InitTree()
        {
            foreach (var node in nodes)
            {
                node.tree = this.tree;
                node.childNodes = new List<Node>();

                foreach (var subNodeGuid in node.childNodeGuid)
                {

                    var subNode = FindNodeWithGuid(subNodeGuid);

                    if (subNode == null)
                    {
                        throw new Exception("The child node owned by the node are not on the tree");
                    }
                    else
                    {
                        node.childNodes.Add(subNode);
                    }
                }

                node.parentNode = FindNodeWithGuid(node.parentNodeGuid);
            }

#if UNITY_EDITOR
            foreach (var line in connectLines)
            {
                line.outNode = FindNodeWithGuid(line.outNodeGuid);
                line.inNode = FindNodeWithGuid(line.inNodeGuid);
            }

            List<Node> tempNodes = new List<Node>();
            foreach (var selectedNode in SelectedNodes)
            {
                var newNode = FindNodeWithGuid(selectedNode.guid);
                if (newNode != null)
                    tempNodes.Add(newNode);
                else
                {
                    throw new Exception("node of selected not exist in nodes id:" +
                        selectedNode.guid + "count:" + SelectedNodes.Count);
                }

            }
            SelectedNodes = tempNodes;

            List<ConnectLine> tempLines = new List<ConnectLine>();
            foreach (var selectedLine in SelectedConnectLines)
            {
                var newLine = FindConnectLineWithGuid(selectedLine.outNodeGuid, selectedLine.inNodeGuid);

                if (newLine != null)
                {
                    tempLines.Add(newLine);
                }
                else
                {
                    throw new Exception("line of selected not exist in lines");
                }
            }
            SelectedConnectLines = tempLines;
#endif

        }

#if UNITY_EDITOR
        public ConnectLine FindConnectLineWithGuid(string outNodeGuid, string inNodeGuid)
        {
            foreach (var line in connectLines)
            {
                if (line.outNodeGuid == outNodeGuid && line.inNodeGuid == inNodeGuid)
                {
                    return line;
                }
            }

            return null;
        }
#endif

    }
}