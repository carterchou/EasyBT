using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(DecoratorNode))]
    public class InverseNode : DecoratorNode
    {

        internal override void Evaluate(Queue<Node> evaluateList)
        {
            if (!HasSubNode())
            {
                this.taskState = TaskState.Success;
                parentNode.ReturnResult(TaskState.Success, evaluateList);
                return;
            }

#if UNITY_EDITOR
            ReSetNodeState();
#endif

            taskState = TaskState.Running;
            childNodes[0].Evaluate(evaluateList);
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            if (TaskState.Success == taskState)
            {
                this.taskState = TaskState.Failure;
                parentNode.ReturnResult(TaskState.Failure,evaluateList);
            }            
            else if(TaskState.Failure == taskState)
            {
                this.taskState = TaskState.Success;
                parentNode.ReturnResult(TaskState.Success,evaluateList);
            }
               
        }
    }
}

