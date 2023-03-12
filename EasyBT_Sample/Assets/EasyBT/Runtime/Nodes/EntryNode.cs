using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    /// <summary>
    /// EntryNode is unique in the tree,The is as an anchor and entry point
    /// </summary>
    public class EntryNode : Node
    {
        internal override void Evaluate(Queue<Node> evaluateList)
        {
            if(this.childNodes.Count ==0)
            {
                base.taskState = TaskState.Success;
                return;
            }

            base.taskState = TaskState.Success;
            childNodes[0].Evaluate(evaluateList);
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            
        }
    }
}