using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(DecoratorNode))]
    public class TimeNode : DecoratorNode
    {
        private float lastTime = 0;
        public float coolDownTime;
        public bool executedFirstTime = false;
        private bool isFirstTime = true;

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

            if (isFirstTime)
            {
                if(executedFirstTime)
                {
                    taskState = TaskState.Running;
                    lastTime = Time.time;
                    childNodes[0].Evaluate(evaluateList);
                }
                else
                {
                    lastTime = Time.time;
                }

                isFirstTime = false;
            }

            if (Time.time <= (lastTime + coolDownTime))
            {
                taskState = TaskState.Failure;
                parentNode.ReturnResult(TaskState.Failure, evaluateList);
            }
            else
            {
                taskState = TaskState.Running;
                lastTime = Time.time;
                childNodes[0].Evaluate(evaluateList);
            }

        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            this.taskState = taskState;
            parentNode.ReturnResult(taskState, evaluateList);
        }
    }
}

