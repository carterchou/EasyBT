using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{

    [CustomNodeAttibute(typeof(CompositeNode))]
    public class SelectNode : CompositeNode
    {

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
            childNodes[currentIndex].Evaluate(evaluateList);
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            if (taskState == TaskState.Failure)
            {

                currentIndex++;

                if (currentIndex > childNodes.Count - 1)
                {
                    parentNode.ReturnResult(TaskState.Failure,evaluateList);
                    return;
                }
                else
                {
                    childNodes[currentIndex].Evaluate(evaluateList);
                    return;
                }

            }
            else
            {
                this.taskState = taskState;
                parentNode.ReturnResult(taskState,evaluateList);
            }
        }
    }
}

