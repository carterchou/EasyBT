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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "SetAnimationFromCurrentSkill")]
    public class SetAnimationFromCurrentSkill : Act
    {
        // Define your parameters here
        public Actor actor;

        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            actor.animator.SetInteger("status", actor.currentSkill.animation);
            if (!actor.animator.GetCurrentAnimatorStateInfo(0).IsName(((AnimationState)actor.currentSkill.animation).ToString()))
                return TaskState.Running;
            else
                return TaskState.Success;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {

        }

    }
}