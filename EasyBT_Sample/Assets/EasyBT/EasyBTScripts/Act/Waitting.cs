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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "Waitting")]
    public class Waitting : Act
    {
        // Define your parameters here
        public float time;
        float currentTime = 0;

        public override void OnStart(BehaviorTree behaviorTree)
        {
            currentTime = 0;
            behaviorTree.StartCoroutine(waitting());
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            return currentTime < time ? TaskState.Running : TaskState.Success;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

        private IEnumerator waitting()
        {
            while(currentTime < time)
            {
                yield return null;
                currentTime += Time.deltaTime;
            }
        }

    }
}