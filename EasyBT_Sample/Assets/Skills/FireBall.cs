using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    // Start is called before the first frame update
    public Actor target;
    public Skill skill;
    public Actor actor;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 forward = (target.transform.position - transform.position).normalized;
        transform.position += forward * Time.deltaTime * 30f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target == null)
        {
            Destroy(this.gameObject, 0.5f);
            target = null;
        }
        else if (other.gameObject == target.gameObject)
        {
            Spawner.Hit(actor, target, skill);
            Destroy(this.gameObject, 0.5f);
            target = null;
        }
    }
}
