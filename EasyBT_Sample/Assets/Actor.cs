using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum AnimationState
{
    Idle,
    Run,
    Walk,
    BasicAttack,
    Die,
    Injured,
    Sitting,
    Cast,

}

public class Actor : MonoBehaviour
{
    public List<Actor> enemies = new List<Actor>();
    public int kb;
    // [HideInInspector]
    public int currentKb;
    // [HideInInspector]
    public Actor target;
    //  [HideInInspector]
    public Vector3 move_pos;
    public float detectionDistance;
    public int maxHp;
    [SerializeField]
    private int _currentHp;
    public int hp
    {
        get { return _currentHp; }
        set
        {
            _currentHp = value; if (hp_slider != null) hp_slider.value = (float)_currentHp / (float)maxHp;
        }
    }
    public int mp;
    public int atk;
    public List<Skill> skills;
    public Skill currentSkill;
    public Transform fireBallPoint;
    public Transform damagePos;
    public Animator animator;

    private float timer;
    public Slider hp_slider;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }


    private void Awake()
    {
        hp = maxHp;
        move_pos = transform.position;
        skills = skills.Select(s => ScriptableObject.Instantiate(s)).ToList();
    }

    private void Update()
    {
        if (target?.hp <= 0)
            target = null;

        enemies = enemies.Where(e => e.hp > 0).ToList();

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sitting")&& _currentHp > 0)
        {
            timer += Time.deltaTime * 3f;

            if (timer >= 1)
            {
                hp += (int)timer;
                timer = 0;
            }
        }
    }

    public bool HasBetterSkill()
    {
        var orderSkills = from skill in skills
                          orderby skill.mp descending
                          select skill;

        foreach (var skill in orderSkills)
        {
            if (Time.time > (skill.cd + skill.lastUsedTime))
            {
                if (skill.mp > currentSkill.mp)
                    return true;
            }
        }

        return false;
    }

    public bool SetSkill()
    {

        var orderSkills = from skill in skills
                          orderby skill.mp descending
                          select skill;

        foreach (var skill in orderSkills)
        {
            if (Time.time > (skill.cd + skill.lastUsedTime) && this.mp >= skill.mp)
            {
                currentSkill = skill;
                return true;
            }
            else
            {
                if (skill.lastUsedTime == 0 && this.mp >= skill.mp)
                {
                    currentSkill = skill;
                    return true;
                }
            }
        }

        Debug.Log("cant find a skill");
        return false;
    }
}
