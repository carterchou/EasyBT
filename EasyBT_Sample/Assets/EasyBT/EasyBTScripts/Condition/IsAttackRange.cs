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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "IsAttackRange")]
    public class IsAttackRange : Act
    {
        // Define your parameters here
        public Actor actor;
        public bool isOutside = true;

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if(isOutside)
            {
                return Vector3.Distance(behaviorTree.transform.position, actor.target.transform.position)
                    > actor.currentSkill.attackDistance ? TaskState.Success : TaskState.Failure;
            }
            else
            {
                return Vector3.Distance(behaviorTree.transform.position, actor.target.transform.position)
                   > actor.currentSkill.attackDistance ? TaskState.Failure : TaskState.Success;
            }
        }

    }
}