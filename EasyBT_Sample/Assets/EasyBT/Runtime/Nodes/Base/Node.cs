using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace EasyBT.Runtime
{
    [Serializable]
    public abstract class Node
#if UNITY_EDITOR
        : IComparable<Node>
#endif
    {
        [NonSerialized]
        public Node parentNode;
        [NonSerialized]
        public List<Node> childNodes = new List<Node>();
        public BehaviorTree tree;

        [TextArea]
        public string description;
        public string guid;
        public string nodeName;
        public string parentNodeGuid;
        public List<string> childNodeGuid = new List<string>();

        [SerializeField]
        private float _probability = 0f;
        public float probability
        {
            get
            {
                
                return _probability;
            }
            set
            {
                _probability = value;
            }
        }

        private TaskState _taskState;
        public TaskState taskState
        {
            get
            {
                return _taskState;
            }
            set
            {
                _taskState = value;
#if UNITY_EDITOR
                nodeStateChanged?.Invoke();
#endif         
            }
        }

#if UNITY_EDITOR
        public int priority { get; set; } = 0;
        [SerializeField]
        public Rect rect;
        public static event Action nodeStateChanged;
#endif

        private List<ActWrap> depthObstacleCondition = new List<ActWrap>();
        public List<ActWrap> obstacleCondition = new List<ActWrap>();

        internal abstract void Evaluate(Queue<Node> evaluateList);

        internal abstract void ReturnResult(TaskState taskState, Queue<Node> evaluateList);

        protected bool ConditionAbortsTest()
        {
            foreach (var condition in obstacleCondition)
            {

                if (condition.act.OnUpdate(this.tree) == TaskState.Success)
                {
                    return true;
                }
            }

            return false;
        }

#if UNITY_EDITOR
        public void ReSetNodeState()
        {
            if(childNodes.Count > 0)
            {
                foreach(var node in childNodes)
                {
                    node.ReSetNodeState();
                }
            }

            taskState = TaskState.None;
        }
        public int CompareTo(Node other)
        {
            return this.priority.CompareTo(other.priority);
        }
#endif

#if UNITY_EDITOR
        public void BreadthTranferObstacle()
        {
            obstacleCondition = new List<ActWrap>();

            if (this.GetType().BaseType.Name == "CompositeNode")
            {
                var compositeNode = ((CompositeNode)this);

                if (compositeNode.obstacleType == CompositeNode.ObstacleType.Priority && parentNode != null)
                {
                    var list = from node in parentNode.childNodes
                               where node.rect.x > this.rect.x
                               select node;

                    foreach (var item in list)
                    {
                        item.depthObstacleCondition = item.depthObstacleCondition.Concat(compositeNode.conditions).ToList();
                    }
                }
            }
        }

        public void DepthTranferObstacle(List<ActWrap> obstacles)
        {
            depthObstacleCondition = obstacles.Concat(depthObstacleCondition).ToList();
            CompositeNode compositeNode = null;
            var isCompositeNode = this.GetType().BaseType.Name == "CompositeNode";

            foreach (var subNode in childNodes)
            {
                if (isCompositeNode)
                {
                    compositeNode = ((CompositeNode)this);

                    if (compositeNode.obstacleType == CompositeNode.ObstacleType.Child)
                    {
                        subNode.DepthTranferObstacle(compositeNode.depthObstacleCondition.Concat(compositeNode.conditions).ToList());

                    }
                    else
                    {
                        subNode.DepthTranferObstacle(depthObstacleCondition);
                    }

                    continue;
                }

                subNode.DepthTranferObstacle(depthObstacleCondition);
            }

            if (isCompositeNode && compositeNode != null && compositeNode.conditions.Count > 0)
                obstacleCondition = depthObstacleCondition.Concat(compositeNode.conditions).ToList();
            else
                obstacleCondition = depthObstacleCondition;
            this.depthObstacleCondition = new List<ActWrap>();
        }
#endif
    }
}

