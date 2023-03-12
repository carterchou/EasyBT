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
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "HasEnemy")]
    public class HasEnemy : Act
    {
        // Define your parameters here
        public Actor actor;
        public bool has;

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if(has)
            {
                foreach (var enemy in actor.enemies)
                {
                    if (enemy.hp > 0)
                        return TaskState.Success;
                }

                return TaskState.Failure;
            }
            else
            {

                foreach (var enemy in actor.enemies)
                {
                    if (enemy.hp > 0)
                        return TaskState.Failure;
                }

                return TaskState.Success;

            }
        }

    }
}