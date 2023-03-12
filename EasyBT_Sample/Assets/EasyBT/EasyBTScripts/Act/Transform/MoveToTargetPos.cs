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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "MoveToTargetPos")]
    public class MoveToTargetPos : Act
    {
        // Define your parameters here
        public Actor actor;
        public float speed;
        public float stopDistance;
        public NavMeshAgent meshAgent;

        public override void OnStart(BehaviorTree behaviorTree)
        {
            meshAgent.isStopped = false;
            meshAgent.speed = speed;
            meshAgent.stoppingDistance = stopDistance;
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {

            meshAgent.destination = actor.move_pos;

            if (!meshAgent.pathPending && meshAgent.remainingDistance < stopDistance)
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