using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    private Actor actor;
    public XftWeapon.XWeaponTrail xWeaponTrail;

    // Start is called before the first frame update
    void Start()
    {
        actor = GetComponent<Actor>(); 
    }

    private void UseSkill()
    {
        if (actor.target == null)
            return;
        var targetActor = actor.target.GetComponent<Actor>();
        actor.currentSkill.lastUsedTime = Time.time;

        if(actor.currentSkill.isSuddenly)
        {
            Spawner.Hit(actor, actor.target, actor.currentSkill);
        }
        else
        {
            var ball = Instantiate(actor.currentSkill.effect,actor.fireBallPoint.position,
               actor.fireBallPoint.rotation).GetComponent<FireBall>();
            ball.target = actor.target;
            ball.skill = actor.currentSkill;
            ball.actor = actor;
        }

        actor.currentSkill = null;
    }

    private void StartWeaponTrail()
    {
        xWeaponTrail.enabled = true;
    }

    private void EndWeaponTrail()
    {
        xWeaponTrail.enabled = false;
    }
}
