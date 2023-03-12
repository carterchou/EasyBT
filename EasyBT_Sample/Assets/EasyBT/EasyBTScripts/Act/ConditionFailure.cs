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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "ConditionFailure")]
    public class ConditionFailure : Act
    {
        // Define your parameters here

        
        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            return TaskState.Failure;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
}