using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyPatrol : MonoBehaviour
{
    public Transform pathHolder;

    public float speed = 5f;
    public float stopTime = 0.5f;
    public float turnSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for(int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = new Vector3(pathHolder.GetChild(i).position.x, transform.position.y, pathHolder.GetChild(i).position.z);
        }
        StartCoroutine(followPath(waypoints));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator followPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if(transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(stopTime);
                yield return StartCoroutine(turnToFace(targetWaypoint));
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator turnToFace(Vector3 lookPoint)
    {
        //float timer = 0f;
        //Vector3 dirToPoint = (lookPoint - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation((lookPoint - transform.position).normalized, transform.up);

        /* while (timer < turnSpeed)
         {
             transform.rotation = Quaternion.Lerp(transform.rotation, rot, timer/turnSpeed);
             timer += Time.deltaTime;
             if(Quaternion.Angle(rot, transform.rotation) <= 0.05f)
             {
                 timer = turnSpeed;
             }
             else
             {
                 yield return null;
             }
         }*/

        while(Quaternion.Angle(rot, transform.rotation) > 0.05f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime * 20f);
            yield return null;
        }
        transform.rotation = rot;
        //transform.rotation = rot;
        yield return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 startPos = pathHolder.GetChild(0).position;
        Vector3 prevPos = startPos;

        foreach(Transform wayPoint in pathHolder)
        {
            Gizmos.DrawSphere(wayPoint.position, 0.3f);
            Gizmos.DrawLine(prevPos, wayPoint.position);
            prevPos = wayPoint.position;
        }
        Gizmos.DrawLine(prevPos, startPos);
    }
}
