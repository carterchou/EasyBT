using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Scripting.APIUpdating;
using EasyBT.Runtime;
using System.Linq;
using System.Linq.Expressions;

namespace EasyBT.Script
{
    //If you want to change the name of the class, please set the name of the original script in the fourth parameter, 
    //otherwise the type will be lost during serialization
    [MovedFrom(false, "EasyBT.Runtime.Script", "Assembly-CSharp", "TryGetTarget")]
    public class TryGetTarget : Act
    {
        // Define your parameters here
        public bool isPlayer;
        public Actor actor;
        
        public override void OnStart(BehaviorTree behaviorTree)
        {

        }

        public override TaskState OnUpdate(BehaviorTree behaviorTree)
        {
            if(isPlayer)
            {
                if(actor.enemies.Count >0)
                {
                    foreach(var enemy in actor.enemies)
                    {
                        if (enemy.hp > 0)
                            actor.target = enemy;
                        return TaskState.Success;
                    }
                }

                var npcList = Spawner.instance?.npcList;
                if (npcList == null)
                    return TaskState.Failure;

                if (npcList.Count == 0)
                    return TaskState.Failure;

                var queryOrder = from e in npcList
                                 orderby Vector3.Distance(e.transform.position, behaviorTree.transform.position)
                                 select e;

                actor.target = queryOrder.First();
                return TaskState.Success;
            }
            else
            {
                var player = Spawner.instance.GetPlayer();

                if(player == null)
                    return TaskState.Running;
                else if (Vector3.Distance(Spawner.instance.GetPlayer().
                    transform.position,behaviorTree.transform.position) <= actor.detectionDistance)
                {
                    actor.target = Spawner.instance.GetPlayer();
                    player.enemies.Add(actor);
                    return TaskState.Success;
                }

                return TaskState.Running;
            }
        }

        public override void OnEnd(BehaviorTree behaviorTree)
        {
       
        }

    }
}