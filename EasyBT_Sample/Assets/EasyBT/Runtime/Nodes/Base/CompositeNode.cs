using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;

namespace EasyBT.Runtime
{
    public abstract class CompositeNode : Node
    {
        public enum ObstacleType
        {
            None = 0,
            Priority = 1,
            Child = 2,
        }

        public List<ActWrap> conditions = new List<ActWrap>();
        public ObstacleType obstacleType = ObstacleType.None;
        protected int currentIndex = 0;

        protected bool ConditionTest()
        {
            foreach(var item in conditions)
            {
                if(item.act.OnUpdate(this.tree) == TaskState.Failure)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool HasSubNode()
        {
            return childNodes.Count == 0 ? false : true;
        }
    }
}

