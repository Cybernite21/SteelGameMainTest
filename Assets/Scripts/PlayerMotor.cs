using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMotor : MonoBehaviour
{

    NavMeshAgent agent; // reference to agent
    Transform target; // target to follow

    public Animator plrAnimator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.position);
            faceTarget();
        }

        //if(agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0.01f)
        if(agent.remainingDistance <= 0.2f)
        {
            plrAnimator.SetInteger("idleOrWalk", 0);
        }
        else
        {
            plrAnimator.SetInteger("idleOrWalk", 1);
        }
    }

    // Update is called once per frame
    public void moveToPoint(Vector3 point)
    {
        agent.SetDestination(point);
    }

    public void followTarget(Interactable newTarget)
    {
        agent.stoppingDistance = newTarget.radius * 0.8f;
        agent.updateRotation = false;

        target = newTarget.interactionTransform;
    }

    public void stopFollowingTarget()
    {
        target = null;
        agent.stoppingDistance = 0;
        agent.updateRotation = true;
    }

    void faceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
}
