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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "SetAnimation")]
    public class SetAnimation : Act
    {
        // Define your parameters here
        public Animator animator;
        public string aniName;
        public int status;
        
        public override void OnStart(BehaviorTree behaviorTree)
        {
            animator.SetInteger("status", status);
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            return animator.GetCurrentAnimatorStateInfo(0).IsName(aniName) ? TaskState.Success : TaskState.Running;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
}