using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(ConditionNode))]
    public class ConditionNode : Node
    {
        public List<ActWrap> conditions = new List<ActWrap>();

        internal override void Evaluate(Queue<Node> evaluateList)
        {
            if (conditions.Count == 0)
            {
                taskState = TaskState.Success;
                this.parentNode.ReturnResult(TaskState.Success, evaluateList);
                return;
            }
            else
            {
                if (ConditionAbortsTest())
                {
                    taskState = TaskState.Failure;
                    this.parentNode.ReturnResult(TaskState.Failure, evaluateList);
                    return;
                }
                else
                {

                    foreach (var item in conditions)
                    {
                        if (item.act.OnUpdate(this.tree) == TaskState.Failure)
                        {
                            taskState = TaskState.Failure;
                            this.parentNode.ReturnResult(TaskState.Failure, evaluateList);
                            return;
                        }
                    }

                    taskState = TaskState.Success;
                    this.parentNode.ReturnResult(TaskState.Success, evaluateList);
                    return;
                }
            }
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            throw new Exception("ReturenResult called from conditionNode");
        }
    }
}