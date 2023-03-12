using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Scripting.APIUpdating;
using EasyBT.Runtime;
using UnityEngine.AI;

namespace EasyBT.Script
{
    //If you want to change the name of the class, please set the name of the original script in the fourth parameter, 
    //otherwise the type will be lost during serialization
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "MoveToTarget")]
    public class MoveToTarget : Act
    {
        // Define your parameters here
        public bool isTargetPos = false;
        public Actor actor;
        public float speed;
        public NavMeshAgent meshAgent;

        public override void OnStart(BehaviorTree behaviorTree)
        {
            meshAgent.isStopped = false;
            meshAgent.speed = speed;
            meshAgent.stoppingDistance = actor.currentSkill.attackDistance;
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            meshAgent.destination = actor.target.transform.position;

            if (!meshAgent.pathPending && meshAgent.remainingDistance < actor.currentSkill.attackDistance)
            {
                return TaskState.Success;
            }
            else
            {
                return TaskState.Running;
            }
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
           meshAgent.isStopped = true;
        }

    }
}