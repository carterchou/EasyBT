using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EasyBT.Runtime;


namespace EasyBT.Runtime
{
    [Serializable]
    public class ConnectLine
    {
        [NonSerialized]
        public Node outNode;
        [NonSerialized]
        public Node inNode;
        public string outNodeGuid;
        public string inNodeGuid;
        public Color color;

        public ConnectLine(Node outNode, Node inNode)
        {
            this.outNode = outNode;
            this.inNode = inNode;
            this.outNodeGuid = outNode.guid;
            this.inNodeGuid = inNode.guid;
            color = Color.white;
        }

        public bool Equals(Node parentNode,Node subNode)
        {
            return (parentNode == outNode && subNode == inNode) ? true : false;
        }
    }
}

