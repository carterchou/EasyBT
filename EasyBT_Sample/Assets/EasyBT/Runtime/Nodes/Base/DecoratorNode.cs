using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    public abstract class DecoratorNode : Node
    {
        protected bool HasSubNode()
        {
            return childNodes.Count == 0 ? false : true;
        }
    }
}

