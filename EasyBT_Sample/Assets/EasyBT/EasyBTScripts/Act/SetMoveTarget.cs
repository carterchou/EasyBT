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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "SetMoveTarget")]
    public class SetMoveTarget : Act
    {
        // Define your parameters here
        public float maximumDistance;
        public NavMeshAgent meshAgent;
        public Actor actor;

        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = behaviorTree.transform.position + UnityEngine.Random.insideUnitSphere * maximumDistance;
                NavMeshHit hit;

                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    actor.move_pos = hit.position;
                    break;
                }
                else
                {
                    actor.move_pos = behaviorTree.transform.position;
                }

            }

            return TaskState.Success;
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
}