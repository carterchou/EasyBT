using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(DecoratorNode))]
    public class FixedCountNode : DecoratorNode
    {
        private int currnetCount;
        public int count;

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

            if (currnetCount >= count)
            {
                taskState = TaskState.Failure;
                parentNode.ReturnResult(TaskState.Failure, evaluateList);
                return;
            }

            taskState = TaskState.Running;
            childNodes[0].Evaluate(evaluateList);
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            ++currnetCount;
            this.taskState = taskState;
            parentNode.ReturnResult(taskState, evaluateList);
        }
    }
}

