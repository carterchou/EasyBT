using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{
    private static Spawner _instance;
    public static Spawner instance { get { return _instance; } private set { _instance = value; } }
    public float interval;
    public int maxCount;

    private float lastSpawnedTime;


    public List<Actor> npcList
    {
        get
        {
            if (_npcList == null)
                _npcList = new List<Actor>();
            return _npcList;
        }
    }
    private List<Actor> _npcList;
    public Actor player;

    public GameObject npc;
    public float maximumDistance;

    public Actor GetPlayer()
    {
        if (player.hp <= 0)
            return null;
        else
            return player;
    }

    private void Awake()
    {
        _instance = this;
        lastSpawnedTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > (interval + lastSpawnedTime))
        {
            Spawn();
            lastSpawnedTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, maximumDistance);
    }

    public void RemoveFromList(Actor actor)
    {
        if (actor == player)
            return;
        else
            _npcList.Remove(actor);
    }

    private void Spawn()
    {
        if (npcList.Count >= maxCount)
            return;

        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * maximumDistance;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                var npcInstance = Instantiate<GameObject>(npc, hit.position, Quaternion.identity);
                npcList.Add(npcInstance.GetComponent<Actor>());
                break;
            }

        }
    }

    public GameObject damageText;

    public static void Hit(Actor source,Actor dest,Skill skill)
    {
        int damage = (int)(source.atk * (skill.damage / 100f));
        dest.hp = (int)Mathf.Clamp(dest.hp - damage, 0, Mathf.Infinity);
        dest.currentKb += damage;
        Instantiate(skill.hit,dest.damagePos.position,Quaternion.identity);
        var text = Instantiate(Spawner.instance.damageText, dest.damagePos.position, Quaternion.identity).GetComponent<Text3D>();
        text.textMeshPro.text = damage.ToString();

        if (dest.target == null)
            dest.target = source;
    }
}
