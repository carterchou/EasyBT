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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "HasMovePos")]
    public class HasMovePos : Act
    {
        // Define your parameters here
        public bool has;
        public Actor actor;

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if (has)
                return (actor.move_pos != actor.transform.position) ? TaskState.Success : TaskState.Failure;
            else
                return (actor.move_pos == actor.transform.position) ? TaskState.Success : TaskState.Failure;
               
        }

    }
}