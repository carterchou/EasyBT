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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "ReSetKB")]
    public class ReSetKB : Act
    {
        // Define your parameters here
        public Actor actor;
        
        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            actor.currentKb = 0;
            return TaskState.Success;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
}