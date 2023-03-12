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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "WaittingAniEventFiredOff")]
    public class WaittingAniEventFiredOff : Act
    {
        // Define your parameters here
        public Animator animator;
        public string waittingEvent;
        private float eventNormalizedTime;

        public override void OnStart(BehaviorTree behaviorTree)
        {
            var clip = animator.GetCurrentAnimatorClipInfo(0)[0];

            foreach (var e in clip.clip.events)
            {
                if (e.functionName == waittingEvent)
                {
                    eventNormalizedTime = e.time / clip.clip.length;
                }
            }
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < eventNormalizedTime)
                return TaskState.Running;
            else
                return TaskState.Success;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {

        }

    }
}