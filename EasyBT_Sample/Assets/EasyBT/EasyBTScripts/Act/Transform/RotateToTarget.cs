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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "RotateToTarget")]
    public class RotateToTarget : Act
    {
        // Define your parameters here
        public bool isTargetPos = false;
        public Actor actor;
        public float speed = 1f;
        public float allowedAngle = 2f;

        private NavMeshAgent meshAgent;
        
        public override void OnStart(BehaviorTree behaviorTree)
        {
            if (meshAgent == null)
                meshAgent = actor.GetComponent<NavMeshAgent>();
            meshAgent.updateRotation = false;
        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            Vector3 targetDirection = Vector3.zero;

            if (isTargetPos)
            {
                targetDirection = (actor.move_pos - actor.transform.position).normalized;
            }
            else
            {
                targetDirection = (actor.target.transform.position - actor.transform.position).normalized;
            }

            if (Vector3.Angle(actor.transform.forward, targetDirection) < allowedAngle)
            {
                return TaskState.Success;
            }
            else
            {
                float singleStep = speed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(actor.transform.forward, targetDirection, singleStep, 0.0f);
                actor.transform.rotation = Quaternion.LookRotation(newDirection);
                return TaskState.Running;
            }
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
            meshAgent.updateRotation = true;
        }

    }
}