using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using EasyBT.Runtime;

namespace EasyBT.Runtime
{

    [Serializable]
    public abstract class Act
    {
        public virtual void OnStart(BehaviorTree behaviorTree)
        {

        }

        public virtual void OnEnd(BehaviorTree behaviorTree)
        {

        }

        public abstract TaskState OnUpdate(BehaviorTree behaviorTree);
    }
}


