using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BTAgentHasPath : BTnode
{
    private NavMeshAgent agent;


    public BTAgentHasPath(NavMeshAgent agent)
    {
        this.agent = agent;
    }

    public override IEnumerator Run(BehaviorTree behaviorTree)
    {
        status = BTstatus.RUNNING;
       
        if (agent.enabled==true && agent.hasPath && agent.remainingDistance >= agent.stoppingDistance && agent.isStopped==false)
            status = BTstatus.SUCCESS;
        else
            status = BTstatus.FAILURE;

        yield break;
        
    }
}
