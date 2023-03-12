using EasyBT.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EasyBT.Editor
{
    public interface INodeInspectionDrawer
    {
        public void Draw(SerializedObject serializedObject, NodeInspector nodeInspector);
    }
}

