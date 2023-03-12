using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{

    [CustomNodeAttibute(typeof(CompositeNode))]
    public class ProbabilityNode : CompositeNode
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

            taskState = TaskState.Running;

            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            float value = UnityEngine.Random.Range(0, 10000);

            foreach (var subNode in childNodes)
            {
                if ((value < (subNode.probability * 100)))
                {
                    subNode.Evaluate(evaluateList);
                    return;
                }
                else
                {
                    value -= (subNode.probability * 100);
                    continue;
                }
            }
        }

        internal override void ReturnResult(TaskState taskState,Queue<Node> evaluateList)
        {
            base.taskState = taskState;
            parentNode.ReturnResult(taskState,evaluateList);
        }
    }
}

