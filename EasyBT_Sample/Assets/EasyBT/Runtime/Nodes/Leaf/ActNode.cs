using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace EasyBT.Runtime
{
    [CustomNodeAttibute(typeof(ActNode))]
    public class ActNode : Node
    {
        public ActWrap actWrap = new ActWrap();
        private bool isInit = false;

        internal override void Evaluate(Queue<Node> evaluateList)
        {
            if (actWrap.act == null)
            {
                this.taskState = TaskState.Failure;
                parentNode.ReturnResult(this.taskState, evaluateList);
                return;
            }
            else
            {

                if (ConditionAbortsTest())
                {
                    if (isInit)
                        actWrap.act.OnEnd(this.tree);
                    isInit = false;
                    this.taskState = TaskState.Failure;
                    parentNode.ReturnResult(TaskState.Failure, evaluateList);
                }
                else
                {
                    if (!isInit)
                    {
                        actWrap.act.OnStart(this.tree);
                        isInit = true;
                    }

                    var result = actWrap.act.OnUpdate(this.tree);

                    if (result == TaskState.Running)
                    {
                        this.taskState = result;
                        evaluateList.Enqueue(this);
                    }
                    else
                    {
                        this.taskState = result;
                        actWrap.act.OnEnd(this.tree);
                        isInit = false;
                        parentNode.ReturnResult(result, evaluateList);
                    }
                }
            }
        }

        internal override void ReturnResult(TaskState taskState, Queue<Node> evaluateList)
        {
            throw new Exception("ReturenResult called from actNode");
        }
    }
}

