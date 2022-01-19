using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
public class BTAgentIsMoving : BTnode
{
    private NavMeshAgent agent;


    public BTAgentIsMoving(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;

        Debug.Log(agent.velocity);

        if (agent.velocity != Vector3.zero)
            status = BTstatus.SUCCESS;
        else
            status = BTstatus.FAILURE;

        yield break;

    }
}
