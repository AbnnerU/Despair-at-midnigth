using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BTRandomNearbyPoint : BTnode
{
    private NavMeshAgent agent;

    private LayerMask layerMask;

    private float maxPointDistance;

    private float minDirectionAngle;

    private float maxDirectionAngle;

    private RaycastHit[] hitResult;

    public BTRandomNearbyPoint(NavMeshAgent agent, float maxPointDistance, float minDirectionAngle, float maxDirectionAngle, LayerMask layerMask)
    {
        this.agent = agent;

        this.maxPointDistance = maxPointDistance;

        this.minDirectionAngle = minDirectionAngle;

        this.maxDirectionAngle = maxDirectionAngle;

        this.layerMask = layerMask;

        hitResult = new RaycastHit[1];
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        float distance = agent.stoppingDistance;

        float angleValue = Random.Range(minDirectionAngle, maxDirectionAngle);

        Quaternion spreadAngle = Quaternion.AngleAxis(angleValue, Vector3.up);

        Vector3 rayDirection = spreadAngle * agent.transform.forward ;

        Ray ray = new Ray(agent.transform.position, rayDirection);

        int hit = Physics.RaycastNonAlloc(ray, hitResult, maxPointDistance, layerMask);

        Vector3 target;

        if (hit > 0)
        {
            target = hitResult[0].point;
        }
        else
        {
            target = ray.GetPoint(maxPointDistance);
        }

        Debug.DrawRay(agent.transform.position, rayDirection);

        Debug.Log("Origem: " + agent.transform.position + " /  Alvo: " + target + "/ Angulo dado: " + angleValue + "/ Angulo real: " + Vector3.Angle(agent.transform.position, target).ToString());

        agent.SetDestination(target);

        agent.isStopped = false;

        while (agent != null && agent.enabled == true)
        {
            //Debug.Log(agent.remainingDistance);
            if (agent.pathPending == false && agent.remainingDistance < distance)
            {
                Debug.Log("DISTANCE REF");
                status = BTstatus.SUCCESS;

                yield break;
            }

            yield return null;
        }

        status = BTstatus.FAILURE;

        yield break;

    }
}