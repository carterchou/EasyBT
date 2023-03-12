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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "HasTarget")]
    public class HasTarget : Act
    {
        // Define your parameters here
        public Actor actor;
        public bool has;

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if (has)
                return (actor.target != null) ? TaskState.Success : TaskState.Failure;
            else
                return (actor.target == null) ? TaskState.Success : TaskState.Failure;
        }

    }
}