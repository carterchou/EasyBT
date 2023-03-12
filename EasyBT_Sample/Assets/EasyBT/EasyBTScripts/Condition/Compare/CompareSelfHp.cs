using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Scripting.APIUpdating;
using EasyBT.Runtime;

namespace EasyBT.Script
{

    //If you want to change the name of the class, please set the name of the original script in the fourth parameter, 
    //otherwise the type will be lost during serialization
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "CompareSelfHp")]
    public class CompareSelfHp : Act
    {
        // Define your parameters here
        public Actor actor;
        public CompareType compareType;
        public int threshold;

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if (compareType == CompareType.Equals)
            {
                if (actor.hp == threshold)
                    return TaskState.Success;
                else
                {
                    return TaskState.Failure;
                }
                  
            }
            else if (compareType == CompareType.Greater)
            {
                if (actor.hp > threshold)
                    return TaskState.Success;
                else
                    return TaskState.Failure;
            }
            else
            {
                if (actor.hp < threshold)
                    return TaskState.Success;
                else
                    return TaskState.Failure;
            }
        }

    }
}