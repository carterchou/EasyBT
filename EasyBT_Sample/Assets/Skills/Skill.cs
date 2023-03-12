using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Skill : ScriptableObject
{
    public string skillName;
    public int mp;
    public float damage;
    public int animation;
    public float cd;
    public float attackDistance;
    public bool isSuddenly = true;

    //[HideInInspector]
    public float lastUsedTime;


    public ParticleSystem hit;
    public ParticleSystem effect;
}
