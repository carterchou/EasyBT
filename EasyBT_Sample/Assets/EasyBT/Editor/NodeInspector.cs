using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using EasyBT.Runtime;
namespace EasyBT.Editor
{
    public class NodeInspector : ScriptableObject
    {
        [SerializeReference]
        public Node currentNode; 
    }
}

