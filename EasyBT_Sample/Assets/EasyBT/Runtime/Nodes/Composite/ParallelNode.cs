using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(CompositeNode))]
    public class ParallelNode : CompositeNode
    {
        private bool hasFailureResult = false;

        internal override void Evaluate(Queue<Node> evaluateList)
        {
            if (!HasSubNode())
            {
                this.taskState = TaskState.Success;
                parentNode.ReturnResult(TaskState.Success, evaluateList);
                return;
            }

            if (obstacleType == ObstacleType.Priority && !ConditionTest())
            {
                parentNode.ReturnResult(TaskState.Failure, evaluateList);
                return;
            }

#if UNITY_EDITOR
            ReSetNodeState();
#endif

            currentIndex = 0;
            taskState = TaskState.Running;
            hasFailureResult = false;

            foreach (var subNode in childNodes)
            {
                subNode.Evaluate(evaluateList);
            }
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            if (taskState == TaskState.Failure)
            {
                hasFailureResult = true;
            }

            currentIndex++;

            if (currentIndex == childNodes.Count)
            {
                var result = hasFailureResult == (true) ? TaskState.Failure : TaskState.Success;
                this.taskState = result;
                parentNode.ReturnResult(result, evaluateList);
            }
        }
    }
}

